namespace COM617.Components.Nav
{
    public partial class MobileNavbar
    {
        private bool collapseNavMenu { get; set; } = true;

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

        public void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
            StateHasChanged();
        }
    }
}
