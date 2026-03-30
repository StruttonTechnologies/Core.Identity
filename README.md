### Core.Identity

A structured identity and authentication framework designed to integrate with the application architecture while enforcing clear boundaries and controlled execution.

Instead of mixing authentication logic directly into controllers or services, identity operations are handled through defined contracts and coordinated execution.

For example, instead of:

var user = await _userManager.FindByNameAsync(request.Username);
if (user == null)
{
    throw new UserNotFoundException();
}

var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

You use coordinated request handling:

var result = await _mediator.Send(new LoginCommand(request.Username, request.Password));

---

### Common Usage Patterns

#### Coordinated request handling

Identity operations are executed through commands and handlers:

return await _mediator.Send(new ChangePasswordCommand(userId, currentPassword, newPassword));

---

#### External login handling

External authentication is handled through a consistent orchestration flow:

return await _mediator.Send(new ExternalLoginCommand(provider, accessToken));

---

#### Token generation

Token creation is handled through orchestration rather than inline logic:

var token = await _tokenOrchestration.GenerateTokenAsync(user);

---

#### User retrieval and queries

Queries are separated from execution logic:

var user = await _mediator.Send(new GetUserByIdQuery(userId));

---

### Why this matters

The identity system enforces:

- clear separation between request, coordination, and execution  
- consistent handling of authentication and user operations  
- controlled access to identity infrastructure (UserManager, SignInManager)  
- predictable and testable flows  

This results in:

- cleaner application code  
- reduced duplication of identity logic  
- improved maintainability  
- stronger architectural boundaries

  ### 🚧 Status

This project is currently in active development. Changes are being made regularly as the architecture and implementation continue to evolve. 
