/*
 * LsmDocument.cs
 * Author: Guido Arbia
 */

using System;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.IO;

namespace RuntimeParseLib.Lsm
{
    public class LsmDocument
    {
        LsmStateCollection _states = new LsmStateCollection();
        LsmState _startState = null;
        int _lastProvidedStateID = -1;

        public LsmDocument()
        {
            _states.CollectionChanged += States_CollectionChanged;
        }

        public int GetStateID()
        {
            return ++_lastProvidedStateID;
        }

        public void States_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                foreach (LsmState newState in e.NewItems)
                {
                    if (newState.Document != this)
                        newState.Document = this;

                    newState.ID = GetStateID();
                }
            else if (e.Action == NotifyCollectionChangedAction.Remove
                || e.Action == NotifyCollectionChangedAction.Replace)
                foreach (LsmState oldState in e.OldItems)
                {
                    if (oldState.Document == this)
                        oldState.Document = null;
                }
            
        }

       

        public LsmState StartState
        {
            get { return _startState; }
            set { _startState = value; }
        }

        public LsmStateCollection States
        {
            get { return _states; }
        }

        public LsmTokenizeFunction BuildLexerFunction()
        {
            LsmContext context = new LsmContext();
            LabelTarget breakTarget = Expression.Label();

            Expression block = Expression.Block(
                context.GetLocalVariables(),
                context.GetInitializationBlock(StartState.ID),
                Expression.Loop(
                    Expression.Block(
                        LsmCommonExpressions.AssignPeekValue(context),
                        Expression.IfThenElse(
                            LsmCommonExpressions.EndOfStream(context),
                            Expression.Empty(),
                            LsmCommonExpressions.AssignCurrentChar(context)),
                        States.GetExpression(context),
                        Expression.IfThenElse(
                            LsmCommonExpressions.EndOfStream(context),
                            Expression.Break(breakTarget),
                            Expression.Empty())),
                    breakTarget),
                    context.currentStateVariable);

             Expression<LsmTokenizeFunction> finalExpression
                = Expression.Lambda<LsmTokenizeFunction>(
                    block,
                    context.textReaderParameter,
                    context.tokenListVariable);

            return finalExpression.Compile();
        }
    }
}
