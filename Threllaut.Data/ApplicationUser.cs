using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;

namespace Threllaut.Data;

[Table("Users", Schema = "identity")]
public class ApplicationUser : IdentityUser<Guid>;
