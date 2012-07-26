﻿using System;

namespace Nohros.Logging
{
  /// <summary>
  /// An implementation of the <see cref="ILogger"/> which forwards all
  /// its methods to another <see cref="ILogger"/> object.
  /// </summary>
  public class ForwardingLogger: IForwardingLogger
  {
    ILogger logger_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="ForwardingLogger"/> using
    /// the specified <see cref="ILogger"/> as backing logger.
    /// </summary>
    /// <param name="logger">
    /// The logger instance that methods are forwarder to.
    /// </param>
    public ForwardingLogger(ILogger logger) {
      logger_ = logger;
    }
    #endregion

    /// <inheritdoc />
    public bool IsDebugEnabled {
      get { return logger_.IsDebugEnabled; }
    }

    /// <inheritdoc />
    public bool IsErrorEnabled {
      get { return logger_.IsErrorEnabled; }
    }

    /// <inheritdoc />
    public bool IsFatalEnabled {
      get { return logger_.IsFatalEnabled; }
    }

    /// <inheritdoc />
    public bool IsInfoEnabled {
      get { return logger_.IsInfoEnabled; }
    }

    /// <inheritdoc />
    public bool IsWarnEnabled {
      get { return logger_.IsWarnEnabled; }
    }

    /// <inheritdoc />
    public bool IsTraceEnabled {
      get { return logger_.IsTraceEnabled; }
    }

    /// <inheritdoc />
    public void Debug(string message) {
      logger_.Debug(message);
    }

    /// <inheritdoc />
    public void Debug(string message, Exception exception) {
      logger_.Debug(message, exception);
    }

    /// <inheritdoc />
    public void Error(string message) {
      logger_.Error(message);
    }

    /// <inheritdoc />
    public void Error(string message, Exception exception) {
      logger_.Error(message, exception);
    }

    /// <inheritdoc />
    public void Fatal(string message) {
      logger_.Fatal(message);
    }

    /// <inheritdoc />
    public void Fatal(string message, Exception exception) {
      logger_.Fatal(message, exception);
    }

    /// <inheritdoc />
    public void Info(string message) {
      logger_.Info(message);
    }

    /// <inheritdoc />
    public void Info(string message, Exception exception) {
      logger_.Info(message, exception);
    }

    /// <inheritdoc />
    public void Warn(string message) {
      logger_.Warn(message);
    }

    /// <inheritdoc />
    public void Warn(string message, Exception exception) {
      logger_.Warn(message, exception);
    }

    /// <summary>
    /// Gets the backing logger instance that methods are forwarder to.
    /// </summary>
    public ILogger Logger {
      get { return logger_; }
      set { logger_ = value; }
    }
  }
}