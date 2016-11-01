using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GigHub.Core.Models
{
    /// <summary>
    /// 跟随关系
    /// </summary>
    public class Following
    {
        [Key]
        [Column(Order = 1)]
        public string FollowerId { get; set; }

        [Key]
        [Column(Order = 2)]
        public string FolloweeId { get; set; }

        //关注者
        public ApplicationUser Follower { get; set; }

        //被关注者
        public ApplicationUser Followee { get; set; }
    }
}