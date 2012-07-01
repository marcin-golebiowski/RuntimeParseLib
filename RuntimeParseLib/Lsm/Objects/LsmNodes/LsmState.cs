/*
 * LsmState.cs
 * Author: Guido Arbia
 */

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace RuntimeParseLib.Lsm
{
    public class LsmState : ILsmExpressable
    {
        LsmDocument _document;
        LsmActionCollection _defaultActions = new LsmActionCollection();
        LsmActionCollection _pastEndActions = new LsmActionCollection();
        LsmMatchRuleCollection _matchRules;
        int _stateID;

        //public event PropertyChangedEventHandler PropertyChanged;

        public LsmState()
        {
            _matchRules = new LsmMatchRuleCollection(this);
        }

        public int ID
        {
            get { return _stateID; }
            set { _stateID = value; }
        }

        public LsmDocument Document
        {
            get { return _document; }
            set
            {
                if (_document != null
                    && _document.States.Contains(this))
                    _document.States.Remove(this);

                _document = value;

                if (_document != null
                    && !_document.States.Contains(this))
                {
                    _document.States.Add(this);
                }
            }
        }

        public LsmMatchRuleCollection MatchRules
        {
            get { return _matchRules; }
        }

        public LsmActionCollection DefaultActions
        {
            get { return _defaultActions; }
        }

        public LsmActionCollection PostStreamActions
        {
            get { return _pastEndActions; }
        }

        public Expression GetBodyExpression(LsmContext lsmContext)
        {
            return Expression.IfThenElse(
                LsmCommonExpressions.EndOfStream(lsmContext),
                PostStreamActions.GetExpression(lsmContext),
                MatchRules.GetExpression(lsmContext));
        }

        public Expression GetExpression(LsmContext lsmContext)
        {
            return Expression.IfThenElse(
                Expression.Equal(
                    lsmContext.currentStateVariable,
                    Expression.Constant(this.ID)),
                GetBodyExpression(lsmContext),
                Expression.Empty());
        }
    }
}
