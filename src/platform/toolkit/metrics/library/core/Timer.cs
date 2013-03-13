﻿using System;

namespace Nohros.Metrics
{
  /// <summary>
  /// A timer metric which aggregates timing durations and provides duration
  /// statistics, plus throughput statistics via <see cref="Meter"/>.
  /// </summary>
  public class Timer : ITimed, ITimer
  {
    readonly Clock clock_;
    internal readonly TimeUnit duration_unit_;
    readonly BiasedHistogram histogram_;
    readonly Meter meter_;

    #region .ctor
    /// <summary>
    /// Creates a new <see cref="Timer"/>.
    /// </summary>
    /// <param name="duration_unit">
    /// The scale unit for this timer's duration metrics.
    /// </param>
    public Timer(TimeUnit duration_unit, Meter meter, BiasedHistogram histogram)
      : this(duration_unit, meter, histogram, new UserTimeClock()) {
    }

    public Timer(TimeUnit duration_unit, Meter meter, BiasedHistogram histogram,
      Clock clock) {
      duration_unit_ = duration_unit;
      meter_ = meter;
      histogram_ = histogram;
      clock_ = clock;
    }
    #endregion

    /// <inheritdoc/>
    public TimeUnit RateUnit {
      get { return meter_.RateUnit; }
    }

    /// <inheritdoc/>
    public string EventType {
      get { return meter_.EventType; }
    }

    public long Count {
      get { return meter_.Count; }
    }

    /// <inheritdoc/>
    public double FifteenMinuteRate {
      get { return meter_.FifteenMinuteRate; }
    }

    /// <inheritdoc/>
    public double FiveMinuteRate {
      get { return meter_.FiveMinuteRate; }
    }

    /// <inheritdoc/>
    public double MeanRate {
      get { return meter_.MeanRate; }
    }

    /// <inheritdoc/>
    public double OneMinuteRate {
      get { return meter_.OneMinuteRate; }
    }

    /// <inheritdoc/>
    public Snapshot Snapshot {
      get {
        double[] values = histogram_.Snapshot.Values;
        var converted = new double[values.Length];
        for (int i = 0, j = values.Length; i < j; i++) {
          converted[i] = ConvertFromNs(values[i]);
        }
        return new Snapshot(converted);
      }
    }

    /// <summary>
    /// Gets the shortest recorded duration.
    /// </summary>
    /// <value>The shortest recorded duration.</value>
    public double Min {
      get { return ConvertFromNs(histogram_.Min); }
    }

    /// <summary>
    /// Gets the longest recorded duration.
    /// </summary>
    /// <value>The longest recorded duration.</value>
    public double Max {
      get { return ConvertFromNs(histogram_.Max); }
    }

    /// <summary>
    /// Gets the arithmetic mean of all recorded durations.
    /// </summary>
    /// <value>The arithmetic mean of all recorded durations.</value>
    public double Mean {
      get { return ConvertFromNs(histogram_.Mean); }
    }

    /// <summary>
    /// Gets the standard deviation of all recorded durations.
    /// </summary>
    /// <value>The standard deviation of all recorded durations.</value>
    public double StandardDeviation {
      get { return ConvertFromNs(histogram_.StandardDeviation); }
    }

    /// <summary>
    /// Adds a recorded duration.
    /// </summary>
    /// <param name="duration">The length of the duration.</param>
    /// <param name="unit">The scale unit of <paramref name="duration"/></param>
    public void Update(long duration, TimeUnit unit) {
      Update(TimeUnitHelper.ToNanos(duration, unit));
    }

    public void Report<T>(MetricReportCallback<T> callback, T context) {
      callback(Report(), context);
    }

    public T Time<T>(CallableDelegate<T> method) {
      long start_time = clock_.Tick;

      // The time should be mensured even if a exception is throwed.
      try {
        return method();
      } finally {
        Update(clock_.Tick - start_time);
      }
    }

    public void Time(RunnableDelegate method) {
      long start_time = clock_.Tick;

      // The time should be mensured even if a exception is throwed.
      try {
        method();
      } finally {
        Update(clock_.Tick - start_time);
      }
    }

    /// <summary>
    /// Gets a timing <see cref="TimerContext"/>, which measures an elapsed
    /// time in nanoseconds.
    /// </summary>
    /// <returns>
    /// A new <see cref="TimerContext"/>.
    /// </returns>
    public TimerContext Time() {
      return new TimerContext(this, clock_);
    }

    protected MetricValue[] Report() {
      Snapshot snapshot = Snapshot;
      string duration_unit = UnitHelper.FromTimeUnit(duration_unit_);
      string rate_unit = UnitHelper.FromRate(EventType, RateUnit);
      return new[] {
        new MetricValue("min", Min, duration_unit),
        new MetricValue("max", Max, duration_unit),
        new MetricValue("mean", Mean, duration_unit),
        new MetricValue("stddev", StandardDeviation, duration_unit),
        new MetricValue("median", snapshot.Median, duration_unit),
        new MetricValue("percentile75", snapshot.Percentile75, duration_unit),
        new MetricValue("percentile95", snapshot.Percentile95, duration_unit),
        new MetricValue("percentile98", snapshot.Percentile98, duration_unit),
        new MetricValue("percentile99", snapshot.Percentile99, duration_unit),
        new MetricValue("percentile999", snapshot.Percentile999, duration_unit),
        new MetricValue("count", Count, EventType),
        new MetricValue("meanRate", MeanRate, rate_unit),
        new MetricValue("oneMinuteRate", OneMinuteRate, rate_unit),
        new MetricValue("fiveMinuteRate", FiveMinuteRate, rate_unit),
        new MetricValue("fifteenMinuteRate", FifteenMinuteRate, rate_unit)
      };
    }

    /// <summary>
    /// Adds a recorded duration.
    /// </summary>
    /// <param name="duration">The length of the duration.</param>
    void Update(long duration) {
      if (duration >= 0) {
        histogram_.Update(duration);
        meter_.Mark();
      }
    }

    double ConvertFromNs(double ns) {
      return ns/TimeUnitHelper.ToNanos(1, duration_unit_);
    }

    /// <summary>
    /// Gets the timer's duration scale unit.
    /// </summary>
    public TimeUnit DurationUnit {
      get { return duration_unit_; }
    }
  }
}
