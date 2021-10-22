import AllIcon from "@material-ui/icons/AllInclusiveOutlined";

export interface TabInterface {
  icon: React.ReactElement<any, string | React.JSXElementConstructor<any>>;
  label: string;
}

const tabs: TabInterface[] = [
  {
    label: "All",
    icon: <AllIcon />,
  },

  {
    label: "Meat",
    icon: <AllIcon />,
  },

  {
    label: "Vegetables",
    icon: <AllIcon />,
  },

  {
    label: "Bakery",
    icon: <AllIcon />,
  },

  {
    label: "Misc",
    icon: <AllIcon />,
  },
];

export default tabs;
