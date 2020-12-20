using System;
using System.Collections.Generic;
using System.Linq;

namespace WebEas.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public class Role 
    {
        private List<Role> subRoles = new List<Role>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Role" /> class.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        public Role(string roleName)
        {
            this.Name = roleName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Role" /> class.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <param name="description">The description.</param>
        public Role(string roleName, string description) : this(roleName)
        {
            this.Description = description;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; private set; }

        /// <summary>
        /// Gets or sets the higher roles.
        /// </summary>
        /// <value>The higher roles.</value>
        public List<Role> SubRoles
        {
            get
            {
                return this.subRoles;
            }
            set
            {
                if (value != this.subRoles)
                {
                    this.subRoles = value;
                }
            }
        }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <value>The roles.</value>
        public HashSet<string> Roles
        {
            get
            {
                var roles = new HashSet<string>();
                this.AddRole(this, roles);
                return roles;
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Adds the role.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="collection">The collection.</param>
        private void AddRole(Role node, HashSet<string> collection)
        {
            collection.Add(node.Name);

            foreach (Role role in node.subRoles)
            {
                this.AddRole(role, collection);
            }
        }

        public static implicit operator string(Role role)
        {
            return role.Name;
        }
    }
}