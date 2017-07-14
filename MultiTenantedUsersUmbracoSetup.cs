using MultiTenantedUsers.Data;
using umbraco.businesslogic;
using umbraco.interfaces;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace MultiTenantedUsers
{
    public class MultiTenantedUsersUmbracoSetup : ApplicationEventHandler
    {
        /// <summary>
        /// When umbraco has started up
        /// </summary>
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            //When Umbraco Started lets check for DB table exists
            var db = applicationContext.DatabaseContext.Database;
            
            if (!db.TableExist(MultiTenantedConstants.MULTITENANCY_TABLE_NAME))
            {
                db.OpenSharedConnection();
                db.CreateTable<MultiTenantedUser>(false);
                var cmd = db.CreateCommand(db.Connection, @"
                    ALTER TABLE multiTenantedUsers WITH CHECK ADD
                        CONSTRAINT FK_MultiTenantedUser_UmbracoUser FOREIGN KEY (identityId)
                        REFERENCES umbracoUser(id) ON DELETE CASCADE,
                        CONSTRAINT FK_MultiTenantedUser_UmbracoNode FOREIGN KEY (orgNodeId)
                        REFERENCES umbracoNode(id) ON DELETE CASCADE
                    ");
                cmd.ExecuteNonQueryWithRetry();
            }

            if (!db.TableExist(MultiTenantedConstants.MULTITENANCY_MEMBERS_TABLE_NAME))
            {
                db.OpenSharedConnection();
                db.CreateTable<MultiTenantedMember>(false);
                var cmd = db.CreateCommand(db.Connection, @"
                    ALTER TABLE multiTenantedMembers WITH CHECK ADD
                        CONSTRAINT FK_MultiTenantedMember_UmbracoMember FOREIGN KEY (memberNodeId)
                        REFERENCES umbracoNode(id),
                        CONSTRAINT FK_MultiTenantedMember_UmbracoNode FOREIGN KEY (orgNodeId)
                        REFERENCES umbracoNode(id)
                    ");
                cmd.ExecuteNonQueryWithRetry();
            }

            //Continue as normal
            base.ApplicationStarted(umbracoApplication, applicationContext);
        }
        
    }

    [Application("orgusers", "Users", "trayuser", 4)]
    public class OrgUsersSectionApplication : IApplication { }
    
}
