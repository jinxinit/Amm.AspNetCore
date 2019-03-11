﻿#region 版权信息
// ------------------------------------------------------------------------------
// Copyright: (c) 2018  梅军章
// 项目名称：JwellFace
// 文件名称：Log4NetLogger.cs
// 版本号: V1.0.0.0
// 创建时间：2018-11-27 11:33
// 更改时间：2018-11-27 11:33
// ------------------------------------------------------------------------------
#endregion

using System;
using log4net;
using Microsoft.Extensions.Logging;

namespace Amm.AspNetCore.Logger.Log4net
{
    /// <inheritdoc />
    /// <summary>
    ///  log4net日志记录
    /// </summary>
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _log;

        /// <summary>
        /// Log4NetLogger
        /// </summary>
        /// <param name="loggerRepository"></param>
        /// <param name="name"></param>
        public Log4NetLogger(string loggerRepository, string name)
        {
            _log = LogManager.GetLogger(loggerRepository, name);
        }

        /// <summary>Writes a log entry.</summary>
        /// <param name="logLevel">日志级别，将按这个级别写不同的日志</param>
        /// <param name="eventId">事件编号.</param>
        /// <param name="state">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a <c>string</c> message of the <paramref name="state" /> and <paramref name="exception" />.</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
            string message = null;
            if (formatter != null)
            {
                message = formatter(state, exception);
            }
            if (!string.IsNullOrEmpty(message) || exception != null)
            {
                switch (logLevel)
                {
                    case LogLevel.Trace:
                    case LogLevel.Debug:
                        _log.Debug(message);
                        break;
                    case LogLevel.Information:
                        _log.Info(message);
                        break;
                    case LogLevel.Warning:
                        _log.Warn(message);
                        break;
                    case LogLevel.Error:
                        _log.Error(message, exception);
                        break;
                    case LogLevel.Critical:
                        _log.Fatal(message, exception);
                        break;
                    case LogLevel.None:
                        break;
                    default:
                        _log.Warn($"遇到未知的日志级别 {logLevel}, 使用Info级别写入日志。");
                        _log.Info(message, exception);
                        break;
                }
            }
        }

        /// <summary>
        ///  Checks if the given <paramref name="logLevel" /> is enabled.
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    return _log.IsDebugEnabled;
                case LogLevel.Information:
                    return _log.IsInfoEnabled;
                case LogLevel.Warning:
                    return _log.IsWarnEnabled;
                case LogLevel.Error:
                    return _log.IsErrorEnabled;
                case LogLevel.Critical:
                    return _log.IsFatalEnabled;
                case LogLevel.None:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
            }
        }

        /// <summary>Begins a logical operation scope.</summary>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns>An IDisposable that ends the logical operation scope on dispose.</returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}