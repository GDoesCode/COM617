namespace COM617.Components.Nav
{
    public partial class DesktopNavbar
    {
        private List<NavItem> navItems { get; set; } = new List<NavItem>();
        private List<NavItem> adminNavItems { get; set; } = new List<NavItem>();

        private NavItem navItemRef
        {
            set
            {
                navItems.Add(value);
            }
        }

        private NavItem adminNavItemRef
        {
            set
            {
                adminNavItems.Add(value);
            }
        }

        private void SetActive(NavItem navItem)
        {
            navItems.First(x => x == navItem).SetActive();
            StateHasChanged();
        }

        private void SetActiveAdmin(NavItem navItem)
        {
            adminNavItems.First(x => x == navItem).SetActive();
            StateHasChanged();
        }
    }
}
