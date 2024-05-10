using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/accounts")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService) => _accountService = accountService;

    [HttpPost("reset")]
    public IActionResult Reset()
    {
        _accountService.Reset();
        return Ok();
    }

    [HttpGet("balance")]
    public IActionResult GetBalance([FromQuery] string accountId)
    {
        if (string.IsNullOrEmpty(accountId))
            return BadRequest("AccountId is required.");

        accountId = accountId.ToLower();
        var balance = _accountService.GetBalance(accountId);

        if (balance.HasValue)
            return Ok(balance.Value);

        return NotFound("Account not found.");
    }

    [HttpPost("event")]
    public IActionResult HandleEvent([FromBody] EventRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.Type))
            return BadRequest("Invalid request: Type is required.");

        switch (request.Type.ToUpper())
        {
            case "DEPOSIT": return HandleDeposit(request);
            case "WITHDRAW": return HandleWithdraw(request);
            case "TRANSFER": return HandleTransfer(request);
            default: return BadRequest("Invalid event type.");
        }
    }

    private IActionResult HandleDeposit(EventRequest request)
    {
        if (string.IsNullOrEmpty(request.Destination) || request.Amount <= 0)
            return BadRequest("Invalid deposit: Destination and valid amount are required.");

        var success = _accountService.Deposit(request.Destination.ToLower(), request.Amount);

        if (!success)
            return BadRequest("Deposit failed.");

        var balance = _accountService.GetBalance(request.Destination.ToLower());
        return Created("", new { destination = new { id = request.Destination, balance } });
    }

    private IActionResult HandleWithdraw(EventRequest request)
    {
        if (string.IsNullOrEmpty(request.Origin) || request.Amount <= 0)
            return BadRequest("Invalid withdraw: Origin and valid amount are required.");

        var success = _accountService.Withdraw(request.Origin.ToLower(), request.Amount);

        if (!success)
            return BadRequest("Withdraw failed.");

        var balance = _accountService.GetBalance(request.Origin.ToLower());
        return Created("", new { origin = new { id = request.Origin, balance } });
    }

    private IActionResult HandleTransfer(EventRequest request)
    {
        if (string.IsNullOrEmpty(request.Origin) || string.IsNullOrEmpty(request.Destination) || request.Amount <= 0)
            return BadRequest("Invalid transfer: Origin, destination, or valid amount is required.");

        var success = _accountService.Transfer(request.Origin.ToLower(), request.Destination.ToLower(), request.Amount);

        if (!success)
            return BadRequest("Transfer failed.");

        var originBalance = _accountService.GetBalance(request.Origin.ToLower());
        var destinationBalance = _accountService.GetBalance(request.Destination.ToLower());
        return Created("", new { origin = new { id = request.Origin, balance = originBalance }, destination = new { id = request.Destination, balance = destinationBalance } });
    }
}
