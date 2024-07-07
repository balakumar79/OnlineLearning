using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Learning.ViewModel.Extension
{
    public static class DataTableExtensions
    {
        public static IHtmlContent DataTableFor<TModel>(this IHtmlHelper<IEnumerable<TModel>> htmlHelper,
                                                        IEnumerable<TModel> data,
                                                        params Expression<Func<TModel, object>>[] columns)
        {
            var table = new TagBuilder("table");
            table.Attributes.Add("style", "witdh:100%");
            table.AddCssClass("display responsive nowrap datatable");

            var thead = new TagBuilder("thead");
            var tr = new TagBuilder("tr");

            foreach (var columnExpression in columns)
            {
                var propertyName = GetPropertyName(columnExpression);
                var th = new TagBuilder("th");
                th.InnerHtml.AppendHtml(propertyName);
                tr.InnerHtml.AppendHtml(th);
            }

            thead.InnerHtml.AppendHtml(tr);
            table.InnerHtml.AppendHtml(thead);

            var tbody = new TagBuilder("tbody");

            foreach (var item in data)
            {
                var trRow = new TagBuilder("tr");
                foreach (var columnExpression in columns)
                {
                    var td = new TagBuilder("td");
                    var value = GetPropertyValue(item, columnExpression);
                    td.InnerHtml.AppendHtml(value.ToString());
                    trRow.InnerHtml.AppendHtml(td);
                }
                tbody.InnerHtml.AppendHtml(trRow);
            }

            table.InnerHtml.AppendHtml(tbody);
            var t = new HtmlString(table.ToString());
            return table;
        }

        private static string GetPropertyName<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            if (expression.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }

            throw new ArgumentException("Expression must be a member expression");
        }

        private static object GetPropertyValue<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression)
        {
            var memberExpression = (MemberExpression)expression.Body;
            var propertyInfo = (PropertyInfo)memberExpression.Member;
            return propertyInfo.GetValue(model);
        }
    }
}
