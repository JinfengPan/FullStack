using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GigHub.Models
{
    public class ApplicationUser : IdentityUser
    {

        public ApplicationUser()
        {
            Followers = new List<Following>();
            Followees = new List<Following>();
        }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// 跟随我的人
        /// </summary>
        public ICollection<Following> Followers { get; set; }

        /// <summary>
        /// 我所跟随的人
        /// </summary>
        public ICollection<Following> Followees { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // 在此处添加自定义用户声明
            return userIdentity;
        }
    }
}