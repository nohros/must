﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Nohros.Collections;

namespace Nohros.Data
{
  /// <summary>
  /// An implementation of the <see cref="IMapper{T}"/> interface that uses
  /// a <see cref="IDataReader"/> to fetch the data to be mapped.
  /// </summary>
  /// <remarks>
  /// This class implements only <see cref="IEnumerable{T}"/> interface.
  ///<para>
  /// Each time the <see cref="IEnumerator.MoveNext"/> method of the
  /// returned <see cref="IEnumerator{T}"/> object is called, the associated
  /// <see cref="IDataReader"/> is advanced to the next record.
  /// </para>
  /// </remarks>
  internal abstract partial class DataReaderMapper<T> : IMapper<T>,
                                                        IForwardOnlyEnumerable
                                                          <T>,
                                                        IDisposable
  {
    bool has_rows_;
    protected internal CallableDelegate<T> loader_;
    internal int[] ordinals_;
    protected internal IDataReader reader_;

    #region .ctor
    protected internal DataReaderMapper() {
      has_rows_ = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataReaderMapper{T}"/>
    /// using the specified <see cref="IDataReader"/>.
    /// </summary>
    /// <param name="reader">
    /// A <see cref="IDataReader"/> containing the data to be mapped.
    /// </param>
    protected DataReaderMapper(IDataReader reader) {
      reader_ = reader;
      has_rows_ = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataReaderMapper{T}"/>
    /// using the specified <see cref="IDataReader"/> and <typeparamref name="T"/>
    /// loader.
    /// </summary>
    /// <param name="reader">
    /// A <see cref="IDataReader"/> containing the data to be mapped.
    /// </param>
    protected DataReaderMapper(IDataReader reader, CallableDelegate<T> loader) {
      reader_ = reader;
      loader_ = loader;
      has_rows_ = false;
    }
    #endregion

    public void Dispose() {
      reader_.Dispose();
    }

    public virtual T Map() {
      T t;
      if (!Map(out t)) {
        throw new NoResultException();
      }
      return t;
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }

    public virtual IEnumerator<T> GetEnumerator() {
      T t;
      while (Map(out t)) {
        yield return t;
      }
    }

    public bool Map(out T t) {
      if (reader_.Read()) {
        t = MapInternal();
        return true;
      }
      t = default(T);
      return false;
    }

    protected internal abstract T MapInternal();

    internal void Initialize(IDataReader reader) {
      Initialize(reader, NewT);
    }

    internal void Initialize(IDataReader reader, CallableDelegate<T> loader) {
      reader_ = reader;
      GetOrdinals();
      if (loader == null) {
        if (ordinals_ == null) {
          loader_ = delegate { throw new NoResultException(); };
        }
      }
      loader_ = loader ?? NewT;
    }

    internal virtual void GetOrdinals() {
      throw new NotImplementedException();
    }

    internal virtual T NewT() {
      throw new NotImplementedException();
    }
  }
}