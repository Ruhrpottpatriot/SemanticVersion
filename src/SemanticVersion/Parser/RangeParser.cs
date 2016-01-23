namespace SemanticVersion.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;

    using SemVer = SemanticVersion.Version;

    public class RangeParser
    {
        private readonly Stack<Expression> expressionStack = new Stack<Expression>();
        private readonly Stack<Symbol> operatorStack = new Stack<Symbol>();

        private readonly ParameterExpression variableExpression = Expression.Parameter(typeof(SemVer));

        public Func<SemVer, bool> Evaluate(string range)
        {
            return this.Parse(range).Compile();
        }

        public Expression<Func<SemVer, bool>> Parse(string range)
        {
            if (string.IsNullOrWhiteSpace(range))
            {
                throw new ArgumentException("The range string must not be null or empty", nameof(range));
            }

            this.expressionStack.Clear();
            this.operatorStack.Clear();

            string copyString = range;

            while (copyString.Length > 0)
            {
                if (copyString[0] == '*' || char.IsDigit(copyString[0]))
                {
                    char[] opArr = { ' ', '|', '&', '!', '=', '<', '>' };
                    string version = copyString.TakeWhile(t => !opArr.Any(c => c.Equals(t))).Aggregate(string.Empty, (current, t) => current + t);

                    copyString = copyString.Substring(version.Length);

                    this.expressionStack.Push(this.variableExpression);
                    this.expressionStack.Push(Expression.Constant(SemVer.Parse(version)));

                    continue;
                }

                int length;
                if (Operation.IsDefined(copyString, out length))
                {
                    Operation currentOp = (Operation)copyString.Substring(0, length);
                    copyString = copyString.Substring(length);

                    this.EvaluateWhile(() =>
                                       this.operatorStack.Count > 0
                                       && this.operatorStack.Peek() != (Parentheses)'('
                                       && currentOp.Precedence >= ((Operation)this.operatorStack.Peek()).Precedence);

                    this.operatorStack.Push(currentOp);
                    continue;
                }

                switch (copyString[0])
                {
                    case '(':
                        copyString = copyString.Substring(1);
                        this.operatorStack.Push(Parentheses.Left);
                        continue;
                    case ')':
                        copyString = copyString.Substring(1);
                        this.EvaluateWhile(() => this.operatorStack.Count > 0 & this.operatorStack.Peek() != Parentheses.Left);
                        this.operatorStack.Pop();
                        continue;
                    case ' ':
                        copyString = copyString.Substring(1);
                        continue;
                    default:
                        throw new ArgumentException($"Encountered invalid character {copyString[0]}");
                }
            }

            this.EvaluateWhile(() => this.operatorStack.Count > 0);

            return Expression.Lambda<Func<SemVer, bool>>(this.expressionStack.Pop(), this.variableExpression);
        }

        [SuppressMessage("ReSharper", "LoopVariableIsNeverChangedInsideLoop")]
        private void EvaluateWhile(Func<bool> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "The loop condition must not be null.");
            }

            while (condition())
            {
                Operation operation = (Operation)this.operatorStack.Pop();

                Expression[] expressions = new Expression[operation.NumberOfOperands];
                for (int i = operation.NumberOfOperands - 1; i >= 0; i--)
                {
                    expressions[i] = this.expressionStack.Pop();
                }

                this.expressionStack.Push(operation.Apply(expressions));
            }
        }
    }
}
