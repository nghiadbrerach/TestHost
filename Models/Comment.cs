//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebEnterprise.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Comment
    {
        public int CommentID { get; set; }
        public string CommentText { get; set; }
        public string AuthorName { get; set; }
        public System.DateTime CommentDate { get; set; }
        public int CTassignID { get; set; }
    
        public virtual ContentAssign ContentAssign { get; set; }
    }
}
