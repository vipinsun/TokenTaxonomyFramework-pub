const options = [
  {
    key: '',
    label: 'sidebar.dashboard',
    leftIcon: 'ion-android-desktop',
  },
  {
    key: 'categories',
    label: 'sidebar.categories',
    leftIcon: 'ion-podium',
    children: [
      {
        key: 'base',
        label: 'sidebar.base',
      },
      {
        key: 'behaviors',
        label: 'sidebar.behaviors',
      },
      {
        key: 'behavior-groups',
        label: 'sidebar.behavior-groups',
      },
      {
        key: 'property-sets',
        label: 'sidebar.property-sets',
      },
      {
        key: 'token-templates',
        label: 'sidebar.token-templates',
      }
    ],
  },
];
export default options;
