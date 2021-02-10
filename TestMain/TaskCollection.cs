using Xunit;

namespace TestMain
{
    [CollectionDefinition("Long Time Task Collection")]
    public class TaskCollection:ICollectionFixture<LongTimeTaskFixture>
    {
    }
}
