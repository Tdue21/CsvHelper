﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvHelper.Tests
{
	[TestClass]
    public class HeaderValidationTests
    {
		[TestMethod]
		public void CorrectHeadersTest()
		{
			using( var csv = new CsvReader( new StringReader( "Id,Name" ) ) )
			{
				csv.Read();
				csv.ReadHeader();
				csv.ValidateHeader<Test>();
			}
		}

		[TestMethod]
		public void PropertiesTest()
		{
			using( var csv = new CsvReader( new StringReader( "bad data" ) ) )
			{
				csv.Read();
				csv.ReadHeader();
				Assert.ThrowsException<ValidationException>( () => csv.ValidateHeader<Test>() );
			}
		}

		[TestMethod]
		public void ReferencesTest()
		{
			using( var csv = new CsvReader( new StringReader( "bad data" ) ) )
			{
				csv.Read();
				csv.ReadHeader();
				Assert.ThrowsException<ValidationException>( () => csv.ValidateHeader<HasReference>() );
			}
		}

		[TestMethod]
		public void ConstructorParametersTest()
		{
			using( var csv = new CsvReader( new StringReader( "bad data" ) ) )
			{
				csv.Read();
				csv.ReadHeader();
				Assert.ThrowsException<ValidationException>( () => csv.ValidateHeader<HasConstructor>() );
			}
		}

		[TestMethod]
		public void PropertiesGetRecordTest()
		{
			using( var csv = new CsvReader( new StringReader( "bad data" ) ) )
			{
				csv.Configuration.ThrowOnBadHeader = true;
				csv.Read();
				Assert.ThrowsException<ValidationException>( () => csv.GetRecord( typeof( Test ) ) );
			}
		}

		[TestMethod]
		public void PropertiesGetRecordGenericTest()
		{
			using( var csv = new CsvReader( new StringReader( "bad data" ) ) )
			{
				csv.Configuration.ThrowOnBadHeader = true;
				csv.Read();
				Assert.ThrowsException<ValidationException>( () => csv.GetRecord<Test>() );
			}
		}

		[TestMethod]
		public void PropertiesGetRecordsTest()
		{
			using( var csv = new CsvReader( new StringReader( "bad data" ) ) )
			{
				csv.Configuration.ThrowOnBadHeader = true;
				Assert.ThrowsException<ValidationException>( () => csv.GetRecords( typeof( Test ) ).ToList() );
			}
		}

		[TestMethod]
		public void PropertiesGetRecordsGenericTest()
		{
			using( var csv = new CsvReader( new StringReader( "bad data" ) ) )
			{
				csv.Configuration.ThrowOnBadHeader = true;
				Assert.ThrowsException<ValidationException>( () => csv.GetRecords<Test>().ToList() );
			}
		}

		private class Test
		{
			public int Id { get; set; }

			public string Name { get; set; }
		}

		private class HasReference
		{
			public Test Reference { get; set; }
		}

		public class HasConstructor
		{
			public int Id { get; private set; }

			public string Name { get; private set; }

			public HasConstructor( int Id, string Name )
			{
				this.Id = Id;
				this.Name = Name;
			}
		}
    }
}
