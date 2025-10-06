using System;
using System.Linq;
using ABCRetail.StorageWeb.Options;
using ABCRetail.StorageWeb.Services;
using Microsoft.Extensions.Options;

namespace ABCRetail.StorageWeb.Tests;

public sealed class StorageConfigurationTests
{
    private const string ExpectedConnectionString = "DefaultEndpointsProtocol=https;AccountName=st10445399;AccountKey=YR3KAdDu3X68htPeoIX1QbkjFV3IO488WvGm+3COk8ETeehKkeJIQko8kq6nWXzwgJ7edp948rpi+AStsxrVNw==;EndpointSuffix=core.windows.net";

    [Fact]
    public void AzureStorageOptions_HasConfiguredDefaults()
    {
        var options = new AzureStorageOptions();

        Assert.Equal(ExpectedConnectionString, options.AccountConnectionString);
        Assert.Equal("customers", options.CustomersTableName);
        Assert.Equal("products", options.ProductsTableName);
        Assert.Equal("mediacontent", options.MediaContainerName);
        Assert.Equal("operations-queue", options.OperationsQueueName);
        Assert.Equal("contracts", options.ContractsFileShareName);
        Assert.Equal("ST10445399", options.PlaceholderStudentNumber);
        Assert.Equal("CLDV6212", options.PlaceholderModuleCode);
    }

    [Fact]
    public void StorageClientFactory_UsesConfiguredResourceNames()
    {
        var fakeKey = Convert.ToBase64String(Enumerable.Repeat((byte)0x42, 32).ToArray());
        var fakeConnectionString = $"DefaultEndpointsProtocol=https;AccountName=fakeaccount;AccountKey={fakeKey};EndpointSuffix=core.windows.net";

        var options = Microsoft.Extensions.Options.Options.Create(new AzureStorageOptions
        {
            AccountConnectionString = fakeConnectionString,
            CustomersTableName = "demo-customers",
            ProductsTableName = "demo-products",
            MediaContainerName = "demo-media",
            OperationsQueueName = "demo-queue",
            ContractsFileShareName = "demo-contracts"
        });

        var factory = new StorageClientFactory(options);

        Assert.Equal("demo-customers", factory.CreateCustomersTableClient().Name);
        Assert.Equal("demo-products", factory.CreateProductsTableClient().Name);
        Assert.Equal("demo-media", factory.CreateMediaContainerClient().Name);
        Assert.Equal("demo-queue", factory.CreateOperationsQueueClient().Name);
        Assert.Equal("demo-contracts", factory.CreateContractsShareClient().Name);
    }
}
