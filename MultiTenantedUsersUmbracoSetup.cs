using MultiTenantedUsers.Data;
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
            
            //If table does not exist
            if (!db.TableExist(MultiTenantedConstants.MULTITENANCY_TABLE_NAME))
            {
                db.OpenSharedConnection();
                db.CreateTable<MultiTenantedUser>(false);
                var cmd = db.CreateCommand(db.Connection, @"
                    ALTER TABLE @0 WITH CHECK ADD
                        CONSTRAINT FK_MultiTenantedUser_UmbracoUser FOREIGN KEY (identityId)
                        REFERENCES umbracoUser(id) ON DELETE CASCADE,
                        CONSTRAINT FK_MultiTenantedUser_UmbracoNode FOREIGN KEY (orgNodeId)
                        REFERENCES umbracoNode(id) ON DELETE CASCADE
                    ", MultiTenantedConstants.MULTITENANCY_TABLE_NAME);
                cmd.ExecuteNonQueryWithRetry();
            }
            
            //Continue as normal
            base.ApplicationStarted(umbracoApplication, applicationContext);
        }
        
    }
}
