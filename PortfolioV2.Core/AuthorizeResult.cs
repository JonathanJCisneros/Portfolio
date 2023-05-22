﻿#pragma warning disable CS8618

namespace PortfolioV2.Core
{
    public class AuthorizeResult
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public bool IsAuthorized
        {
            get
            {
                return Id != null;
            }
        }
    }
}
