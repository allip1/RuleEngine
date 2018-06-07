using System;
using System.Collections.Generic;

namespace BRE
{
	public class Context
    {
        Dictionary<String, IValue> SymbolTable;

        public Context()
        {
			SymbolTable = new Dictionary<String, IValue>();
        }

		public void AddVariableValue(string name, double value)
        {
			Dictionary.Add(name.ToLower(), new NumberValue(value));
		}

		public double GetVariableValue(string name)
        {
			return SymboleTable[name.ToLower()];
		}
    }

    public class ExecutionEngine
    {
		

        public ExecutionEngine()
        {
        }
    }
}
