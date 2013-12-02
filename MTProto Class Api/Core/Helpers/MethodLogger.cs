using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTProto_Class_Api.Core.Helpers
{
    /// <summary>
    /// Options used when logging a method
    /// </summary>
    [Flags]
    public enum LogOptions
    {
        /// <summary>
        /// Log entry into the method
        /// </summary>
        Entry = 0x01,
        /// <summary>
        /// Log exit from the method
        /// </summary>
        Exit = 0x02,
        /// <summary>
        /// Log the execution time of the method
        /// </summary>
        ExecutionTime = 0x04,
        /// <summary> 
        /// Log all data 
        /// </summary> 
        All = 0xFF
    }


    public class MethodLogger : IDisposable
    {
        /// <summary>
        /// Log method entry 
        /// </summary>
        /// <param name="methodName">The name of the method being logged</param>
        /// <param name="options">The log options</param>
        /// <param name="source">The TraceSource that events are written to</param>
        /// <returns>A disposable object or none if logging is disabled</returns>
        public static IDisposable Log(string methodName,
                                      LogOptions options)
        {
            IDisposable logger = null;

            // Check if ExecutionTime logging is requested, and if so log if Verbose 
            // logging (or greater) is chosen
            bool shouldCreate = (options & LogOptions.ExecutionTime) == LogOptions.ExecutionTime;

            // If not logging ExecutionTime, see if ActivityTracing is on, and if so 
            // log only if Entry or Exit tracing is requested
            if (!shouldCreate)
                shouldCreate = (((options & LogOptions.Entry) == LogOptions.Entry) | ((options & LogOptions.Exit) == LogOptions.Exit));

            // Check if we actually need to log anything
            if (shouldCreate)
                logger = new MethodLogger(methodName, options);

            // Will return null if no method logger was needed - which will 
            // effectively be ignored by a using statement.
            return logger;
        }

        /// <summary>
        /// Ctor now private - just called from the static Log method
        /// </summary>
        /// <param name="methodName">The name of the method being logged</param>
        /// <param name="options">The log options</param>        
        private MethodLogger(string methodName, LogOptions options)
        {
            _methodName = methodName;
            _options = options;

            if ((_options & LogOptions.ExecutionTime) == LogOptions.ExecutionTime)
            {
                _sw = new Stopwatch();
                _sw.Start();
            }

            if ((_options & LogOptions.Entry) == LogOptions.Entry)
                Debug.WriteLine("[MethodLogger] Enter method " + methodName);
        }

        /// <summary>
        /// Tidy up
        /// </summary>
        public void Dispose()
        {
            if ((_options & LogOptions.ExecutionTime) == LogOptions.ExecutionTime)
            {
                _sw.Stop();
                 Debug.WriteLine("[MethodLogger] Method {0} execution time {1}ms",
                                         _methodName,
                                         _sw.ElapsedMilliseconds);
            }

            if ((_options & LogOptions.Exit) == LogOptions.Exit)
                Debug.WriteLine("[MethodLogger] Exit method " + _methodName);
        }

        private string _methodName;
        private LogOptions _options;
        private Stopwatch _sw;  
    }
}
