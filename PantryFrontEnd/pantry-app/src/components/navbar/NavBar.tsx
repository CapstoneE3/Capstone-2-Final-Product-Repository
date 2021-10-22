import {
  Button,
  CircularProgress,
  List,
  ListItem,
  ListItemIcon,
  ListItemText,
  makeStyles,
  Theme,
  useTheme,
} from "@material-ui/core";
import Drawer from "@material-ui/core/Drawer";
import { ExitToApp } from "@material-ui/icons";
import AccountIcon from "@material-ui/icons/AccountCircle";
import InventoryIcon from "@material-ui/icons/AllInbox";
import QRIcon from "@material-ui/icons/CropFreeOutlined";
import ExpiredBinIcon from "@material-ui/icons/DeleteOutline";
import RecipeIcon from "@material-ui/icons/MenuBookOutlined";
import SettingsIcon from "@material-ui/icons/Settings";
import ShoppingListIcon from "@material-ui/icons/ShoppingCartOutlined";
import { createStyles } from "@material-ui/styles";
import React, { ReactElement } from "react";
import { Link } from "react-router-dom";
import Account from "../pages/account/Account";
import ExpiredBin from "../pages/expiredbin/ExpiredBin";
import Inventory from "../pages/inventory/Inventory";
import QR from "../pages/qrcode/QR";
import Recipe from "../pages/recipe/Recipe";
import ShoppingList from "../pages/shoppinglist/ShoppingList";
import { pageToIndex } from "../routingTable";

export type navItem = {
  icon: ReactElement<any, any>;
  component: React.FC<any>;
  descText: string;
  link: string;
};

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    paper: {
      // background: theme.palette.background.default,
      background: theme.palette.primary.dark,
      width: theme.spacing(30),
      padding: `${theme.spacing(2)}px ${theme.spacing(2)}px`,
      color: "white",
    },
    root: {
      "&.Mui-selected": {
        // backgroundColor: theme.palette.primary.light,
        backgroundColor: theme.palette.primary.main,
        // color: theme.palette.primary.dark,
        color: "white",
        fontWeight: 700,
        fontSize: "1.15rem",
        transition: "color 250ms ease-out",
        borderRadius: theme.spacing(1),
        marginTop: theme.spacing(4),
        marginBottom: theme.spacing(4),
        borderColor: theme.palette.primary.main,
        outline: `${theme.spacing(1)} solid ${theme.palette.primary.main}`,
        outlineColor: theme.palette.primary.main,
        letterSpacing: 1.5,
        "&:hover": {
          color: theme.palette.primary.light,
          "& svg": {
            color: theme.palette.primary.light,
          },
        },
        "& svg": {
          fontWeight: 700,
          color: "white",
        },
      },
      marginTop: theme.spacing(2),
      marginBottom: theme.spacing(2),
      // color: theme.palette.text.primary,
      color: theme.palette.primary.light,
      fontSize: "1rem",
    },
    icon: {
      color: theme.palette.primary.dark,
    },

    logOutButton: {
      textAlign: "center",
      color: theme.palette.secondary.light,
      marginTop: "auto",
      fontWeight: 700,
    },

    logOutButtonRoot: {
      display: "flex",
      justifyContent: "center",
      alignItems: "center",
    },
  })
);

interface NavbarProps {
  currNavTab: number;
  setNavTab: React.Dispatch<React.SetStateAction<number>>;
  drawerVariant: "permanent" | "persistent" | "temporary" | undefined;
  setNavOpen: React.Dispatch<React.SetStateAction<boolean>>;
  open?: boolean;
  handleLogout: (callbackFn: () => void) => void;
}

const NavBar: React.FC<NavbarProps> = ({
  currNavTab,
  setNavTab,
  drawerVariant,
  setNavOpen,
  open = false,
  handleLogout,
}) => {
  const theme = useTheme();
  const classes = useStyles(theme);

  const [isLoggingOut, setLoggingOut] = React.useState(false);

  const handleNavClick = (
    e: React.MouseEvent<HTMLDivElement, MouseEvent>,
    i: number | undefined
  ) => {
    if (i !== undefined) {
      setNavTab(i);
    }
  };

  const handleLogoutClick = () => {
    setLoggingOut(true);
    handleLogout(() => setLoggingOut(false));
  };

  return (
    <Drawer
      variant={drawerVariant}
      open={open}
      anchor="left"
      color={theme.palette.primary.dark}
      classes={{ paper: classes.paper }}
      onClose={() => setNavOpen(false)}
    >
      <List component="nav">
        {navItems.map((item, i) => {
          // if (
          //   navItems.length - 1 !== pageToIndex(item.link) &&
          //   navItems.length - 2 !== pageToIndex(item.link)
          // )
          return (
            <Link
              key={item.link}
              to={item.link}
              style={{ textDecoration: "none" }}
            >
              <ListItem
                button
                selected={currNavTab === pageToIndex(item.link)}
                onClick={(e) => handleNavClick(e, pageToIndex(item.link))}
                alignItems="center"
                classes={{ selected: classes.root }}
                className={classes.root}
              >
                <ListItemIcon style={{ color: theme.palette.primary.light }}>
                  {item.icon}
                </ListItemIcon>
                <ListItemText primary={item.descText} disableTypography />
              </ListItem>
            </Link>
          );
          // return <></>;
        })}
        <ListItem key={420}>
          <Button
            color="secondary"
            variant="contained"
            className={classes.logOutButton}
            classes={{ root: classes.logOutButtonRoot }}
            onClick={handleLogoutClick}
            fullWidth
          >
            <ListItemIcon>
              {isLoggingOut ? (
                <CircularProgress
                  variant="indeterminate"
                  style={{ color: theme.palette.secondary.light }}
                />
              ) : (
                <ExitToApp style={{ color: theme.palette.secondary.light }} />
              )}
            </ListItemIcon>
            <ListItemText>Logout</ListItemText>
          </Button>
        </ListItem>
      </List>
    </Drawer>
  );
};

export default NavBar;

export const navItems: navItem[] = [
  {
    descText: "Inventory",
    link: "/inventory",
    component: Inventory,
    icon: <InventoryIcon />,
  },

  {
    descText: "Shopping List",
    link: "/shoplist",
    component: ShoppingList,
    icon: <ShoppingListIcon />,
  },

  {
    descText: "Expired Bin",
    link: "/expiredbin",
    component: ExpiredBin,
    icon: <ExpiredBinIcon />,
  },

  {
    descText: "Recipes",
    link: "/recipes",
    component: Recipe,
    icon: <RecipeIcon />,
  },

  {
    descText: "QR Scan",
    link: "/qrscan",
    component: QR,
    icon: <QRIcon />,
  },

  {
    descText: "Settings",
    link: "/settings",
    component: React.Fragment,
    icon: <SettingsIcon />,
  },

  {
    descText: "Account",
    link: "/account",
    component: Account,
    icon: <AccountIcon />,
  },
  // {
  //   descText: "Login",
  //   link: "/login",
  //   component: Login,
  //   icon: <AccountIcon />,
  // },
  // {
  //   descText: "Signup",
  //   link: "/signup",
  //   component: Signup,
  //   icon: <AccountIcon />,
  // },
];
