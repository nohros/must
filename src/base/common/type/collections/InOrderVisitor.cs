using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Collections
{
  /// <summary>
  /// A visitor that visits single objects in order.
  /// </summary>
  /// <typeparam name="T">The type of objects to be visited.</typeparam>
  /// <remarks>This class only wraps the methods of the <see cref="IVisitor{T}"/>.
  /// It is useful for classes that could be traversed in different
  /// orders(PreOrder, PostOrder, InOrder, ...)</remarks>
  public class InOrderVisitor<T>: IVisitor<T>
  {
    readonly IVisitor<T> visitor_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="InOrderVisitor"/> class by
    /// using the specified visitor.
    /// </summary>
    /// <param name="visitor">The visitor to use when visiting the object.</param>
    /// <exception cref="ArgumentNullException"><paramref name="visitor"/> is
    /// a null reference.</exception>
    public InOrderVisitor(IVisitor<T> visitor) {
      if (visitor == null)
        throw new ArgumentNullException("visitor");
      visitor_ = visitor;
    }
    #endregion

    /// <summary>
    /// Visits the specified object.
    /// </summary>
    /// <param name="obj">The object to visit.</param>
    /// <param name="state">A user-defined object that qualifies or contains
    /// information about the visitor's
    /// current state.</param>
    public void Visit(T obj, object state) {
      visitor_.Visit(obj, state);
    }

    /// <summary>
    /// Gets a value indicating whether the visitor is done performing your
    /// work.
    /// </summary>
    /// <value><c>true</c> if the visitor is done performing your work;
    /// otherwise <c>false</c>.</value>
    /// <remarks>
    /// This property is typically used in collection traversal operations.
    /// In some situations a collection need to be traversed(in some order) and
    /// the traversal operation must be stoped at some point. A visitor could
    /// inform the visited collection about the stop point by setting the
    /// value of this property to false. The visited collection can check the
    /// value of this property for each visited element and then determine
    /// when the traversal operation must be stoped.
    /// </remarks>
    public bool IsCompleted {
      get { return visitor_.IsCompleted; }
    }
  }

  /// <summary>
  /// A visitor that visits compound objects in order. Compound objects are
  /// objects that are identified by more than one object.<example>A node
  /// within a <see cref="IDictionary{TKey,TValue}"/></example>
  /// </summary>
  /// <typeparam name="T1">The type of objects to be visited.</typeparam>
  /// <remarks>This class only wraps the methods of the
  /// <see cref="IVisitor&lt;T&gt;"/>. It is useful for classes that could be
  /// traversed in different orders(PreOrder, PostOrder, InOrder, ...).
  /// </remarks>
  public class InOrderVisitor<T1, T2>: IVisitor<T1, T2>
  {
    IVisitor<T1, T2> visitor_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="InOrderVisitor{T}"/> class
    /// by using the specified visitor.
    /// </summary>
    /// <param name="visitor">The visitor to use when visiting the object.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="visitor"/> is
    /// a null reference.</exception>
    public InOrderVisitor(IVisitor<T1, T2> visitor) {
      if (visitor == null) {
        throw new ArgumentNullException("visitor");
      }
      visitor_ = visitor;
    }
    #endregion

    /// <summary>
    /// Visits the specified object.
    /// </summary>
    /// <param name="obj1">The first component of the object to visit.</param>
    /// <param name="obj2">The second component of the object to visit.</param>
    /// <param name="state">A user-defined object that qualifies or contains
    /// information about the visitor's current state.</param>
    public void Visit(T1 obj1, T2 obj2, object state) {
      visitor_.Visit(obj1, obj2, state);
    }

    /// <summary>
    /// Gets a value indicating whether the visitor is done performing your
    /// work.
    /// </summary>
    /// <value><c>true</c> if the visitor is done performing your work;
    /// otherwise <c>false</c>.</value>
    /// <remarks>
    /// This property is typically used in collection traversal operations. In
    /// some situations a collection need to be traversed(in some order) and
    /// the traversal operation must be stoped at some point. A visitor could
    /// inform the visited collection about the stop point, by setting the
    /// value of this property to false. The visited collection can check the
    /// value of this property for each visited element and then determine when
    /// the traversal operation must be stoped.
    /// </remarks>
    public bool IsCompleted {
      get { return visitor_.IsCompleted; }
    }
  }
}