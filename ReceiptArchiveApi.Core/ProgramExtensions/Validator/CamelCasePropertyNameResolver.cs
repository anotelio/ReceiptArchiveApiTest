using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using FluentValidation.Internal;

namespace ReceiptArchiveApi.Core.ProgramExtensions.Validator
{
    public static class CamelCasePropertyNameResolver
    {
        public static string ResolvePropertyName(MemberInfo memberInfo, LambdaExpression expression)
        {
            return ToCamelCase(DefaultPropertyNameResolver(memberInfo, expression));
        }

        private static string DefaultPropertyNameResolver(MemberInfo memberInfo, LambdaExpression expression)
        {
            if (expression is not null)
            {
                var chain = PropertyChain.FromExpression(expression);
                if (chain.Count > 0)
                {
                    return chain.ToString();
                }
            }

            if (memberInfo is not null)
            {
                return memberInfo.Name;
            }

            return null;
        }

        public static string ToCamelCase(this string str)
        {
            var words = str.Split(new[] { '_', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var leadWord = Regex.Replace(words[0], @"([A-Z])([A-Z]+|[a-z0-9]+)($|[A-Z]\w*)",
                m => m.Groups[1].Value.ToLower() + m.Groups[2].Value.ToLower() + m.Groups[3].Value);

            var tailWords = words.Skip(1)
                .Select(word => char.ToUpper(word[0]) + word[1..]);

            return leadWord + string.Concat(tailWords);
        }
    }
}
