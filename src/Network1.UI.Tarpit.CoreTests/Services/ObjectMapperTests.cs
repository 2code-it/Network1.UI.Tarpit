using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Network1.UI.Tarpit.Core.Services.Tests
{
	[TestClass]
	public class ObjectMapperTests
	{
		[TestMethod]
		public void Map_When_SourceContainsMatchingProperties_Expect_PropertiesSetOnTarget()
		{
			ObjectMapper mapper = new ObjectMapper();
			MapItem1 item1 = new MapItem1
			{
				Int = 1,
				Long = 2,
				Double = 3,
				Byte = 4,
				NullableByte = 5,
				String = "String",
				Object = new()
			};

			MapItem2 item2 = mapper.Map<MapItem2>(item1);

			Assert.AreEqual(item1.Int, item2.Int);
			Assert.AreEqual(item1.Long, item2.Long);
			Assert.AreEqual(item1.Double, item2.Double);
			Assert.AreEqual(item1.Byte, item2.Byte);
			Assert.AreEqual(item1.NullableByte, item2.NullableByte);
			Assert.AreEqual(item1.String, item2.String);
			Assert.AreNotEqual(item1.Object, item2.Object);
		}

		private class MapItem1
		{
			public int Int { get; set; }
			public long Long { get; set; }
			public double Double { get; set; }
			public byte Byte { get; set; } = 4;
			public byte? NullableByte { get; set; }
			public string? String { get; set; }
			public object? Object { get; set; }
		}

		private class MapItem2
		{
			public int Int { get; set; }
			public long Long { get; set; }
			public double Double { get; set; }
			public byte Byte { get; set; }
			public byte? NullableByte { get; set; }
			public string? String { get; set; }
			public object? Object { get; set; }
		}
	}
}