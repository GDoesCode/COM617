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

        private void SetActive(NavItem newActiveNavItem)
        {
            foreach (var navItem in navItems.Where(x => x != newActiveNavItem))
            {
                navItem.SetActive(false);
            }
            newActiveNavItem.SetActive(true);
            StateHasChanged();
        }

        private void SetActiveAdmin(NavItem newActiveNavItem)
        {
            foreach (var navItem in adminNavItems.Where(x => x != newActiveNavItem))
            {
                navItem.SetActive(false);
            }
            newActiveNavItem.SetActive(true);
            StateHasChanged();
        }

        public void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
            StateHasChanged();
        }
    }
}
