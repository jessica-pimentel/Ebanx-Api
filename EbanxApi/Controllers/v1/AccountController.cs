using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService) => _accountService = accountService;

    [HttpPost("reset")]
    public IActionResult Reset()
    {
        _accountService.Reset();
        return new ContentResult
    {
        Content = "OK",
        StatusCode = 200,
        ContentType = "text/plain"
    };
    }

    [HttpGet("balance")]
    public IActionResult GetBalance([FromQuery] string account_id)
    {
        if (string.IsNullOrEmpty(account_id))
            return NotFound(0);

        account_id = account_id.ToLower();
        var balance = _accountService.GetBalance(account_id);

        if (balance.HasValue)
            return Ok(balance.Value);

        return NotFound(0);
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
            return NotFound(0);

        var balance = _accountService.GetBalance(request.Origin.ToLower());
        return Created("", new { origin = new { id = request.Origin, balance } });
    }

    private IActionResult HandleTransfer(EventRequest request)
    {
        if (string.IsNullOrEmpty(request.Origin) || string.IsNullOrEmpty(request.Destination) || request.Amount <= 0)
            return BadRequest("Invalid transfer: Origin, destination, or valid amount is required.");

        var success = _accountService.Transfer(request.Origin.ToLower(), request.Destination.ToLower(), request.Amount);

        if (!success)
            return NotFound(0);

        var originBalance = _accountService.GetBalance(request.Origin.ToLower());
        var destinationBalance = _accountService.GetBalance(request.Destination.ToLower());
        return Created("", new { origin = new { id = request.Origin, balance = originBalance }, destination = new { id = request.Destination, balance = destinationBalance } });
    }
}