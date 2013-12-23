using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIRLab.Thornado
{
    public class ADSpecification
    {
        protected ADSpecification(string name)
        {
            Name = name;
        }
        public readonly string Name;

        public override string ToString()
        {
            return Name;
        }
        
    }

    public class ADSpecification<T> : ADSpecification
    {
        public ADSpecification(string name) : base(name)
        {
            
        }
        public ADDefinition Define(T value) { return new ADDefinition(this, value); }

    }

    public static class AD
    {
        public static readonly ADSpecification<string> Caption = new ADSpecification<string>("Caption");
        public static readonly ADSpecification<string> Description = new ADSpecification<string>("Description");
    }
}
