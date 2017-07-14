using System;
using System.Linq;
using umbraco.BusinessLogic.Actions;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;
using System.Net.Http.Formatting;
using MultiTenantedUsers.Implementation;

namespace MultiTenantedUsers.Controllers
{
    [PluginController("MultiTenantedUsers")]
    [Tree("orgmembers", "orgmembersTree", "All App Users")]
    public class OrgMembersTreeController : TreeController
    {
        private const string ROOT_NODE_ID = "-1";
        private const string ROOT_MEMBERS_ID = "root-members";
        public IMultiTenancyRepository MultiTenancyRepository { get; set; }

        public OrgMembersTreeController()
        {
            MultiTenancyRepository = new MultiTenancyRepository();
        }

        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            int? orgNodeId = MultiTenancyRepository.GetOrgIdForUser(Security.GetUserId());
            if (orgNodeId == null)
            {
                throw new Exception("Current user does not have permissions to manage this organisation's app users.");
            }

            var nodes = new TreeNodeCollection();
            if (id == ROOT_NODE_ID)
            {
                var membersRoot = CreateTreeNode(ROOT_MEMBERS_ID, id, queryStrings, "App Users", "icon-user", true, "orgmembers");
                nodes.Add(membersRoot);
            }
            else if (id == ROOT_MEMBERS_ID)
            {
                var orgMembers = MultiTenancyRepository.GetOrganisationMembers(orgNodeId.Value).ToList();
                orgMembers.ForEach(member =>
                    nodes.Add(CreateTreeNode(member.Id.ToString(), ROOT_MEMBERS_ID, queryStrings, member.Name, "icon-user", false))
                );
            }

            return nodes;
        }

        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            var menu = new MenuItemCollection();
            if (id == ROOT_NODE_ID)
            {

            }
            else if (id == ROOT_MEMBERS_ID)
            {
                menu.Items.Add<ActionNew>("Add New App User");
                menu.DefaultMenuAlias = ActionNew.Instance.Alias;
            }
            else
            {
                menu.Items.Add<ActionDisable>("Disable");
                menu.DefaultMenuAlias = ActionDisable.Instance.Alias;

            }
            return menu;
        }
        
    }
}