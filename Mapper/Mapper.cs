using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Mapper
{
    public class Mapper
    {
        public static TResult Map<TSource, TResult(TSource source) where TResult : new()
        {
            var result = new TResult();

            PropertyDescriptorCollection pSourceColl = TypeDescriptor.GetProperties(typeof(TSource));
            PropertyDescriptorCollection pResultColl = TypeDescriptor.GetProperties(typeof(TResult));

            TResult obj;
            Object colVal;
            string field1 = "";
            string field2 = "";

            obj = new TResult();

            for (int iResult = 0; iResult < pResultColl.Count; iResult++)
            {
                PropertyDescriptor propResult = pResultColl[iResult];
                field1 = propResult.Name;
                for (int ix = 0; ix < pResultColl.Count; ix++)
                {
                    PropertyDescriptor propSource = pSourceColl[ix];

                    field2 = propSource.Name;

                    if (field1 == field2)
                    {
                        colVal = propSource.GetValue(source) ?? null;
                        propResult.SetValue(obj, colVal);
                    }
                }
            }
            return obj;
        }

        // for mapping a list of TSource object to TResult object
        public static List<TResult> Map<TSource, TResult>(IList<TSource> sourceList) where TSource : ISource, TResult : IResult, new()
        {
            var result = new List<TResult>(sourceList.Count);

            PropertyDescriptorCollection pSourceColl = TypeDescriptor.GetProperties(typeof(TSource));
            PropertyDescriptorCollection pResultColl = TypeDescriptor.GetProperties(typeof(TResult));

            TResult obj;
            Object colVal;
            string field1 = "";
            string field2 = "";

            foreach (TSource item in sourceList)
            {
                obj = new TResult();

                for (int iResult = 0; iResult < pResultColl.Count; iResult++)
                {
                    PropertyDescriptor propResult = pResultColl[iResult];
                    field1 = propResult.Name;
                    for (int ix = 0; ix < pResultColl.Count; ix++)
                    {
                        PropertyDescriptor propSource = pSourceColl[ix];

                        field2 = propSource.Name;

                        if (field1 == field2)
                        {
                            colVal = propSource.GetValue(item) ?? null;
                            propResult.SetValue(obj, colVal);
                        }
                    }
                }

                result.Add(obj);
            }
            return result;
        }

       
    }
}
