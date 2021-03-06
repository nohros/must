﻿using System;
using System.Threading;
using Nohros.Extensions;
using Nohros.Logging;
using Nohros.Resources;

namespace Nohros.Concurrent
{
  /// <summary>
  /// Defines a way to schedule a task in a non-reentrant manner.
  /// </summary>
  /// <remarks>
  /// The <see cref="NonReentrantSchedule"/> is non-reentrant. When a task
  /// is running the <see cref="NonReentrantSchedule"/> wait it to finish
  /// before start a new one. After a task is finished the
  /// <see cref="NonReentrantSchedule"/> waits a predefined 
  /// interval and start the task again.
  /// <para>
  /// The tasks runs in a background thread.
  /// </para>
  /// <para>
  /// Any unhandled exception is catched and published through the
  /// <see cref="ExceptionThrown"/> event. If that are no subscribers to the
  /// <see cref="ExceptionThrown"/> event, the exception will be logged
  /// through the configured <see cref="MustLogger"/> and swallowed.
  /// </para>
  /// </remarks>
  public class NonReentrantSchedule
  {
    const string kClassName = "Nohros.Concurrent.NonReentrantSchedule";

    readonly TimeSpan interval_;
    readonly ManualResetEvent signaler_;
    bool already_started_;
    TimeSpan delay_;
    Action<object> task_;

    /// <summary>
    /// Initializes a new instance of the <see cref="NonReentrantSchedule"/>
    /// class by using the given interval.
    /// </summary>
    NonReentrantSchedule(TimeSpan interval) {
      interval_ = interval;
      signaler_ = new ManualResetEvent(false);
      already_started_ = false;
      task_ = null;
    }

    /// <summary>
    /// Returns a <see cref="NonReentrantSchedule"/> class than run a task
    /// at every <see cref="interval"/>.
    /// </summary>
    /// <param name="interval">
    /// The interval to schedule a task.
    /// </param>
    /// <returns>
    /// A <see cref="NonReentrantSchedule"/> object that runs a task at every
    /// <see cref="interval"/>.
    /// </returns>
    /// <remarks>
    /// The returned <see cref="NonReentrantSchedule"/> will use a dedicated
    /// thread to run the task.
    /// </remarks>
    public static NonReentrantSchedule Every(TimeSpan interval) {
      return new NonReentrantSchedule(interval);
    }

    /// <summary>
    /// Defines the action that should run at the associated interval.
    /// </summary>
    /// <param name="task">
    /// The action that should run at the associated interval.
    /// </param>
    /// <param name="use_thread_pool">
    /// A value that inidcates if the task should be executed in a thread
    /// from the <see cref="ThreadPool"/> or in a dedicated thread; should
    /// be <c>true</c> to use threads from <see cref="ThreadPool"/> and
    /// <c>false</c> to use a dedicated thead. Default to <c>false</c>
    /// </param>
    [Obsolete("Use the Run method.", true)]
    public void Runnable(Action task, bool use_thread_pool = false) {
      Runnable(obj => task(), null);
    }

    /// <summary>
    /// Defines the action that should run at the associated interval.
    /// </summary>
    /// <param name="task">
    /// The action that should run at the associated interval.
    /// </param>
    /// <param name="use_thread_pool">
    /// A value that inidcates if the task should be executed in a thread
    /// from the <see cref="ThreadPool"/> or in a dedicated thread; should
    /// be <c>true</c> to use threads from <see cref="ThreadPool"/> and
    /// <c>false</c> to use a dedicated thead. Default to <c>false</c>
    /// </param>
    public void Run(Action task, bool use_thread_pool = false) {
      Run(obj => task(), null);
    }

    /// <summary>
    /// Defines the action that should run at the associated interval.
    /// </summary>
    /// <param name="task">
    /// The action that should run at the associated interval.
    /// </param>
    /// <param name="delay">
    /// The time to wait before run the first task.
    /// </param>
    /// <param name="use_thread_pool">
    /// A value that inidcates if the task should be executed in a thread
    /// from the <see cref="ThreadPool"/> or in a dedicated thread; should
    /// be <c>true</c> to use threads from <see cref="ThreadPool"/> and
    /// <c>false</c> to use a dedicated thead. Default to <c>false</c>
    /// </param>
    public void Run(Action task, TimeSpan delay, bool use_thread_pool = false) {
      Run(obj => task(), delay, null);
    }

    /// <summary>
    /// Defines the action that should run at the associated interval.
    /// </summary>
    /// <param name="task">
    /// The action that should run at the associated interval.
    /// </param>
    /// <param name="state">
    /// An object containing data to be used bu the scheduled task.
    /// </param>
    /// <param name="use_thread_pool">
    /// A value that inidcates if the task should be executed in a thread
    /// from the <see cref="ThreadPool"/> or in a dedicated thread; should
    /// be <c>true</c> to use threads from <see cref="ThreadPool"/> and
    /// <c>false</c> to use a dedicated thead. Default to <c>false</c>
    /// </param>
    [Obsolete("Use the Run method.", true)]
    public void Runnable(Action<object> task, object state,
      bool use_thread_pool = false) {
      Run(task, TimeSpan.Zero, state);
    }

    /// <summary>
    /// Defines the action that should run at the associated interval.
    /// </summary>
    /// <param name="task">
    /// The action that should run at the associated interval.
    /// </param>
    /// <param name="state">
    /// An object containing data to be used bu the scheduled task.
    /// </param>
    /// <param name="use_thread_pool">
    /// A value that inidcates if the task should be executed in a thread
    /// from the <see cref="ThreadPool"/> or in a dedicated thread; should
    /// be <c>true</c> to use threads from <see cref="ThreadPool"/> and
    /// <c>false</c> to use a dedicated thead. Default to <c>false</c>
    /// </param>
    public void Run(Action<object> task, object state,
      bool use_thread_pool = false) {
      Run(task, TimeSpan.Zero, state);
    }

    /// <summary>
    /// Defines the action that should run at the associated interval.
    /// </summary>
    /// <param name="task">
    /// The action that should run at the associated interval.
    /// </param>
    /// <param name="state">
    /// An object containing data to be used bu the scheduled task.
    /// </param>
    /// <param name="delay">
    /// The time to wait before running the task for the first time.
    /// </param>
    /// <param name="use_thread_pool">
    /// A value that inidcates if the task should be executed in a thread
    /// from the <see cref="ThreadPool"/> or in a dedicated thread; should
    /// be <c>true</c> to use threads from <see cref="ThreadPool"/> and
    /// <c>false</c> to use a dedicated thead. Default to <c>false</c>
    /// </param>
    public void Run(Action<object> task, TimeSpan delay, object state,
      bool use_thread_pool = false) {
      if (already_started_) {
        throw new InvalidOperationException("The task is already defined.");
      }
      already_started_ = true;
      task_ = task;
      delay_ = delay;

      if (!use_thread_pool) {
        new BackgroundThreadFactory()
          .CreateThread(Run)
          .Start(state);
      } else {
        ThreadPool
          .RegisterWaitForSingleObject(signaler_, ThreadPoolMain, state,
            interval_, true);
      }
    }

    void ThreadPoolMain(object state, bool timed_out) {
      // Execute the task only if the handle has timed out, otherwise the
      // scheduler is stopped.
      if (timed_out) {
        Run(state);

        ThreadPool
          .RegisterWaitForSingleObject(signaler_, ThreadPoolMain, state,
            interval_, true);
      }
    }

    void Run(object state) {
      // Delay the execution for teh first time.
      if (delay_ > TimeSpan.Zero) {
        if (signaler_.WaitOne(delay_)) {
          return;
        }
      }

      // Run the task until the scheduler is stopped.
      do {
        // Ensure that the scheduler will not be stop if an unhandled
        // exception is thrown. If the user wants to stop the scheduler
        // they can subscribe to the ExceptionThrown event and stop the
        // scheduler from there.
        try {
          task_(state);
        } catch (Exception e) {
          OnExceptionThrown(e);
        }
      } while (!signaler_.WaitOne(interval_));
    }

    void OnExceptionThrown(Exception exception) {
      if (ExceptionThrown != null) {
        // Ensure that a exception raised by the method that is handling
        // the first exception does dot crash the scheduler.
        try {
          ExceptionThrown(exception);
        } catch (Exception ex) {
          MustLogger.ForCurrentProcess.Error(
            StringResources.Log_ThrowsException.Fmt("Scheduled Task",
              kClassName), ex);
        }
      } else {
        MustLogger.ForCurrentProcess.Error(
          StringResources.Log_ThrowsException.Fmt("Scheduled Task",
            kClassName), exception);
      }
    }

    /// <summary>
    /// Raised when an exception is thrown while executing the scheduled
    /// task.
    /// </summary>
    public event Action<Exception> ExceptionThrown;

    /// <summary>
    /// Stops scheduling tasks and returns an <seealso cref="WaitHandle"/> that
    /// can be used to monitor when the pending task finish.
    /// </summary>
    /// <remarks>
    /// If there is no task running <seealso cref="WaitHandle.WaitOne()"/>
    /// overloads returns imediatelly.
    /// </remarks>
    public WaitHandle Stop() {
      signaler_.Set();
      return signaler_;
    }
  }
}
