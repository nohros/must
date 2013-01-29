﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NUnit.Framework;
using Nohros.Data;
using Nohros.Dynamics;
using Telerik.JustMock;

namespace Nohros.Common
{
  public class MapperTests
  {
    public class KeyedMapperTest
    {
      public string Name { get; set; }
    }

    public class MapperDerivedTest : MapperTest
    {
    }

    public class MapperTest
    {
      public string Name { get; set; }
    }

    public class IgnoreMapperTest
    {
      public string Name { get; set; }
    }

    public class NestedMapperTest
    {
      public MapperTest Nested { get; set; }
    }

    public class MapNestedMapperTest
    {
      public MapperTest Nested { get; set; }
    }

    public class MapMapperTest
    {
      public string Name { get; set; }
    }

    [Test]
    public void ShouldBuildDynamicType() {
      var reader = Mock.Create<IDataReader>();
      var mapper = new DataReaderMapper<MapperTest>.Builder()
        .Map("usuario_nome", "name")
        .Build(reader);
      Dynamics_.AssemblyBuilder.Save("test.dll");
      Assert.That(mapper, Is.AssignableTo<DataReaderMapper<MapperTest>>());
      Assert.That(mapper, Is.AssignableTo<MapperTest>());
    }

    [Test]
    public void ShouldBuildNestedDynamicType() {
      var reader = Mock.Create<IDataReader>();
      Mock
        .Arrange(() => reader.GetOrdinal(Arg.AnyString))
        .Returns(0);
      var mapper = new DataReaderMapper<NestedMapperTest>.Builder()
        .Map("usuario_nome", "name")
        .Build(reader);
      var inner = mapper.Map().Nested;
      Assert.That(inner, Is.AssignableTo<DataReaderMapper<MapperTest>>());
      Assert.That(inner, Is.AssignableTo<MapperTest>());
      Assert.That(mapper, Is.AssignableTo<DataReaderMapper<NestedMapperTest>>());
      Assert.That(mapper, Is.AssignableTo<NestedMapperTest>());
    }

    [Test]
    public void GetDynamicType() {
      var builder = new DataReaderMapper<NestedMapperTest>.Builder()
        .Map("usuario_nome", "name");
      Type type = builder.GetDynamicType();
      Dynamics_.AssemblyBuilder.Save("test.dll");
    }

    [Test]
    public void ShouldBuildDerivedInterface() {
      var reader = Mock.Create<IDataReader>();
      var mapper = new DataReaderMapper<MapperDerivedTest>.Builder()
        .Build(reader);
      Assert.That(mapper,
        Is.AssignableTo<DataReaderMapper<MapperDerivedTest>>());
      Assert.That(mapper, Is.AssignableTo<MapperDerivedTest>());
      Assert.That(mapper, Is.AssignableTo<MapperTest>());
    }

    [Test]
    public void ShouldIgnoreProperty() {
      var reader = Mock.Create<IDataReader>();
      var mapper = new DataReaderMapper<IgnoreMapperTest>.Builder()
        .Ignore("name")
        .Build(reader);
      Assert.That(() => mapper.Map().Name,
        Throws.TypeOf<NotImplementedException>());
    }

    [Test]
    public void ShouldMapCustomColumnToProperty() {
      var builder = new SqlConnectionStringBuilder();
      builder.DataSource = "192.168.203.186";
      builder.UserID = "nohros";
      builder.Password = "Noors03";

      using (var conn = new SqlConnection(builder.ToString()))
      using (var cmd = new SqlCommand("select 'nohros' as usuario_nome", conn)) {
        conn.Open();
        using (var reader = cmd.ExecuteReader()) {
          var mapper = new DataReaderMapper<MapMapperTest>.Builder()
            .Map("name", "usuario_nome")
            .Build(reader);
          reader.Read();
          Assert.That(mapper.Map().Name, Is.EqualTo("nohros"));
        }
      }
    }

    [Test]
    public void ShouldMapArrayOfKeyValuePairs() {
      var builder = new SqlConnectionStringBuilder();
      builder.DataSource = "192.168.203.186";
      builder.UserID = "nohros";
      builder.Password = "Noors03";

      using (var conn = new SqlConnection(builder.ToString()))
      using (var cmd = new SqlCommand("select 'nohros' as usuario_nome", conn)) {
        conn.Open();
        using (var reader = cmd.ExecuteReader()) {
          var mapper = new DataReaderMapper<KeyedMapperTest>.Builder(
            new KeyValuePair<string, string>[] {
              new KeyValuePair<string, string>("name", "usuario_nome")
            })
            .Build(reader);
          reader.Read();
          Assert.That(mapper.Map().Name, Is.EqualTo("nohros"));
        }
      }
    }

    [Test]
    public void ShouldMapNestedInterface() {
      var builder = new SqlConnectionStringBuilder();
      builder.DataSource = "192.168.203.186";
      builder.UserID = "nohros";
      builder.Password = "Noors03";

      using (var conn = new SqlConnection(builder.ToString()))
      using (var cmd = new SqlCommand("select 'nohros' as usuario_nome", conn)) {
        conn.Open();
        using (var reader = cmd.ExecuteReader()) {
          var derived = new DataReaderMapper<MapNestedMapperTest>.Builder()
            .Build(reader, ()=> {
              return new MapNestedMapperTest {
                Nested = new DataReaderMapper<MapperTest>.Builder()
                  .Map("name", "usuario_nome")
                  .Build(reader)
                  .Map()
              };
            });
          reader.Read();
          Assert.That(derived.Map().Nested.Name, Is.EqualTo("nohros"));
        }
      }
    }

    [Test]
    public void ShoudMapToConstValues() {
      var reader = Mock.Create<IDataReader>();
      var mapper = new DataReaderMapper<IgnoreMapperTest>.Builder()
        .Map("name", new ConstStringMapType("myname"))
        .Build(reader);
      Dynamics_.AssemblyBuilder.Save("test.dll");
      Assert.That(mapper.Map().Name, Is.EqualTo("myname"));
    }
  }
}
