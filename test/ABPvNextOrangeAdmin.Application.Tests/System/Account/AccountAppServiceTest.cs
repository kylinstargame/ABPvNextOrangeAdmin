using System;
using System.Net;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace ABPvNextOrangeAdmin.System.Account;

public sealed class AccountAppServiceTest : ABPvNextOrangeAdminApplicationTestBase
{
    private readonly IAccountAppService _accountAppService;

    public AccountAppServiceTest()
    {
        _accountAppService = GetRequiredService<IAccountAppService>();
    }

    [Fact]
    public async Task GetCaptchaImageAsync()
    {
        var result = await _accountAppService.GetCaptchaImageAsync();
        result.Code.ShouldBe<long>((long) HttpStatusCode.OK);
        result.Data.ImgBytes.ShouldNotBeNull();
    }
}