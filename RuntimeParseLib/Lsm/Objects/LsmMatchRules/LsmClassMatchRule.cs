/*
 * LsmConstantMatchRule.cs
 * Author: Guido Arbia
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace RuntimeParseLib.Lsm
{
    public enum LsmCharacterClass
    {
        Letter,
        Digit,
        Alphanumeric,
        WhiteSpace
    }

    public class LsmClassMatchRule : LsmMatchRule, ILsmExpressable
    {
        LsmCharacterClass _characterClass;

        public LsmClassMatchRule(LsmCharacterClass characterClass)
        {
            _characterClass = characterClass;
        }

        private Expression GetCharacterMatchExpression(LsmContext context)
        {
            switch (_characterClass)
            {
                case LsmCharacterClass.Letter:
                    return LsmCommonExpressions.LetterMatch(context);
                case LsmCharacterClass.Digit:
                    return LsmCommonExpressions.DigitMatch(context);
                case LsmCharacterClass.Alphanumeric:
                    return LsmCommonExpressions.AlphaNumericMatch(context);
                case LsmCharacterClass.WhiteSpace:
                    return LsmCommonExpressions.WhiteSpaceMatch(context);
                default:
                    return Expression.Empty();
            }
        }

        public Expression GetExpression(LsmContext lsmContext)
        {
            return Expression.IfThenElse(
                GetCharacterMatchExpression(lsmContext),
                Actions.GetExpression(lsmContext),
                Expression.Empty());
        }

        public LsmCharacterClass CharacterClass
        {
            get { return _characterClass; }
        }

        public override string ToString()
        {
            return string.Format("Match Class {0}", _characterClass);
        }
    }
}
