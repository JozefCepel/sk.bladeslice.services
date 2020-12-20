using System;
using System.Linq;
using ServiceStack.Logging;

namespace WebEas.ServiceModel
{
    public class EgovLogFactory : ILogFactory
    {
        private readonly bool debugEnabled;

        public EgovLogFactory(bool debugEnabled = true)
        {
            this.debugEnabled = debugEnabled;
        }

        public ILog GetLogger(Type type)
        {
            return new EgovLogger(type) { IsDebugEnabled = this.debugEnabled };
        }

        public ILog GetLogger(string typeName)
        {
            return new EgovLogger(typeName) { IsDebugEnabled = this.debugEnabled };
        }
    }
}