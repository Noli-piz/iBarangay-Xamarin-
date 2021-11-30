using AndroidX.Fragment.App;
using Java.Lang;
using System.Collections.Generic;

namespace iBarangayApp
{
    public class PagerAdapter : FragmentStatePagerAdapter
    {
        public List<AndroidX.Fragment.App.Fragment> fragments = new List<AndroidX.Fragment.App.Fragment>();
        public List<string> fragmentTitles = new List<string>();
        public PagerAdapter(AndroidX.Fragment.App.FragmentManager fm) : base(fm)
        {

        }
        public void AddFragment(AndroidX.Fragment.App.Fragment fragment, string title)
        {
            fragments.Add(fragment);
            fragmentTitles.Add(title);
        }
        public override int Count => fragments.Count;
        public override AndroidX.Fragment.App.Fragment GetItem(int position)
        {
            return fragments[position];
        }
        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(fragmentTitles[position]);
        }
    }
}