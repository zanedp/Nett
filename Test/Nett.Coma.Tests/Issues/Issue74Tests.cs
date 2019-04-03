using System.IO;
using FluentAssertions;
using Xunit;

namespace Nett.Coma.Tests.Issues
{
    public sealed class Issue74Tests
    {
        [Fact]
        public void Foox()
        {
            File.WriteAllText("test.toml",
                @"
[[Items]]
Key = 'val'
[[Items]]
Key = 1");
            var cfg = Config.CreateAs()
                .MappedToType(() => new Cfg())
                .StoredAs(store => store
                    .File("test.toml"))
                .Initialize();

            var items = cfg.Get(c => c.Items);
            cfg.

            items.Should().BeEmpty();
        }

        public class Cfg
        {
            public CfgItem[] Items { get; set; } = new CfgItem[0];
        }

        public class CfgItem
        {
            public string Key { get; set; } = "def";
        }
    }
}
