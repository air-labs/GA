using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIRLab.Thornado
{
    public static class Extensions
    {
        public static T CommonAD<T>(this IList<T> list)
        {
            throw new Exception("Do not invoke this method. It is only purposed for syntax sugar in ExpressionToAddressConverter");
        }
        
        public static T CommonAD<K, T>(this IDictionary<K, T> dict)
        {
            throw new Exception("Do not invoke this method. It is only purposed for syntax sugar in ExpressionToAddressConverter");
        }
     
		public static Q Assume<T,Q>(this T obj)
			where Q : T
		{ 
			throw new Exception("Do not invoke this method. It is only purposed for syntax sugar in ExpressionToAddressConverter");
		}
    }
}
