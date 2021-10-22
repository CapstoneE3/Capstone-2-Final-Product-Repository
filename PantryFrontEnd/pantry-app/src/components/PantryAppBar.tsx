import {
  AppBar,
  FormControl,
  Hidden,
  IconButton,
  makeStyles,
  Menu,
  MenuItem,
  Switch,
  TextField,
  Theme,
  Typography,
  createStyles,
  useTheme,
  Slide,
  useScrollTrigger,
} from "@material-ui/core";
import HamburgerMenuIcon from "@material-ui/icons/Menu";
import React from "react";
import SearchIcon from "@material-ui/icons/SearchOutlined";
import FilterIcon from "@material-ui/icons/TuneOutlined";

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    appBar: {
      padding: 10,
      display: "flex",
      flexDirection: "row",
      justifyContent: "flex-end",
      alignItems: "center",
      width: "auto",
      left: theme.spacing(30),
      backgroundColor: theme.palette.background.default,
      borderBottom: "3px solid " + theme.palette.primary.dark,
      [theme.breakpoints.down("sm")]: {
        left: 0,
      },
    },

    titleTypography: {
      color: theme.palette.primary.dark,
      [theme.breakpoints.down("sm")]: {
        fontSize: 25,
        marginLeft: theme.spacing(1),
      },
    },

    searchInput: {
      marginLeft: "auto",
    },

    openMenuButtonContainer: {
      [theme.breakpoints.down("xs")]: {
        marginRight: "auto",
      },
    },

    ascendingTrack: {
      backgroundColor: theme.palette.secondary.main,
    },

    switchThumb: {
      // backgroundColor: theme.palette.secondary.main,
    },
    switchBase: {
      "& .MuiSwitch-thumb": {
        backgroundColor: theme.palette.secondary.main,
        // "&$checked": {
        //   backgroundColor: theme.palette.primary.main,
        // },
      },
      "&$checked .MuiSwitch-thumb": {
        backgroundColor: theme.palette.primary.main,
      },
    },
    checked: {},
    textRoot: {
      right: 0,
      top: 0,
      width: "auto",
      borderWidth: 10,
      transition: "width 250ms ease-in-out",
      float: "right",
      paddingLeft: theme.spacing(1),
      paddingRight: theme.spacing(1),
    },

    title: {
      fontSize: "2rem",
      alignSelf: "center",
      marginRight: "auto",
      marginLeft: theme.spacing(4),
      fontWeight: "bolder",
      letterSpacing: 3,
    },
  })
);

interface PantryAppBarProps {
  title: string;
  handleOpenMenu: () => void;
  handleSearchClick: (searchTerm: string) => void;
  handleSortTypeChosen: (sortType: number, desc: boolean) => void;
  handleSortDirectionChange: (sortType: number, desc: boolean) => void;
  sortByExpiry?: boolean;
  sortByName?: boolean;
  sortByCategory?: boolean;
  sortByQuantity?: boolean;
}

export enum SORT_TYPES {
  byExpiryDate,
  byName,
  byCategory,
  byQuantity,
}

const PantryAppBar: React.FC<PantryAppBarProps> = ({
  title,
  handleOpenMenu,
  handleSearchClick,
  handleSortDirectionChange,
  handleSortTypeChosen,
  sortByExpiry = false,
  sortByName = false,
  sortByCategory = false,
  sortByQuantity = false,
}) => {
  const theme = useTheme();
  const classes = useStyles(theme);

  const [searchTerm, setSearchTerm] = React.useState("");

  const [filterMenuOpen, setFilterMenuOpen] = React.useState(false);

  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);

  const [sortDescending, setSortDescending] = React.useState(true);

  const [sortType, setSortType] = React.useState(0);

  const trigger = useScrollTrigger();
  return (
    <Slide appear={false} direction="down" in={!trigger}>
      <AppBar
        color="default"
        variant="elevation"
        position="fixed"
        className={classes.appBar}
        classes={{ root: classes.appBar }}
      >
        <Hidden mdUp>
          <IconButton
            onClick={handleOpenMenu}
            color="primary"
            className={classes.openMenuButtonContainer}
          >
            <HamburgerMenuIcon style={{ color: theme.palette.primary.dark }} />
          </IconButton>
        </Hidden>
        <Hidden xsDown>
          <Typography
            variant={"h2"}
            classes={{ root: classes.titleTypography }}
            color="textPrimary"
            className={classes.title}
          >
            {title}
          </Typography>
        </Hidden>

        <TextField
          placeholder="Search for an item"
          size="medium"
          color="primary"
          variant="outlined"
          classes={{ root: classes.textRoot }}
          onChange={(e) => setSearchTerm(e.target.value)}
          InputProps={{
            classes: {
              root: classes.searchInput,
            },
            endAdornment: (
              <IconButton onClick={() => handleSearchClick(searchTerm)}>
                <SearchIcon />
              </IconButton>
            ),
          }}
        />

        <IconButton
          onClick={(e) => {
            setFilterMenuOpen(true);
            setAnchorEl(e.currentTarget);
          }}
        >
          <FilterIcon />
        </IconButton>

        <Menu
          variant="menu"
          open={filterMenuOpen}
          anchorEl={anchorEl}
          keepMounted
          onClose={() => setFilterMenuOpen(false)}
        >
          <FormControl>
            <MenuItem
              style={{ cursor: "default" }}
              onClick={() => {
                setSortDescending((prev) => !prev);
                handleSortDirectionChange(sortType, !sortDescending);
              }}
            >
              Ascending
              <Switch
                color="primary"
                checked={sortDescending}
                classes={{
                  track: classes.ascendingTrack,
                  thumb: classes.switchThumb,
                  switchBase: classes.switchBase,
                  checked: classes.checked,
                }}
              />
              Descending
            </MenuItem>
          </FormControl>
          {sortByExpiry && (
            <MenuItem
              value="expiry"
              key={SORT_TYPES.byExpiryDate}
              onClick={() => {
                setSortType(SORT_TYPES.byExpiryDate);
                handleSortTypeChosen(SORT_TYPES.byExpiryDate, sortDescending);
              }}
              tabIndex={SORT_TYPES.byExpiryDate}
              selected={sortType === SORT_TYPES.byExpiryDate}
            >
              By expiry date
            </MenuItem>
          )}
          {sortByName && (
            <MenuItem
              value="name"
              key={SORT_TYPES.byName}
              tabIndex={SORT_TYPES.byName}
              selected={sortType === SORT_TYPES.byName}
              onClick={() => {
                setSortType(SORT_TYPES.byName);
                handleSortTypeChosen(SORT_TYPES.byName, sortDescending);
              }}
            >
              By name
            </MenuItem>
          )}
          {sortByCategory && (
            <MenuItem
              value="category"
              key={SORT_TYPES.byCategory}
              tabIndex={SORT_TYPES.byCategory}
              selected={sortType === SORT_TYPES.byCategory}
              onClick={() => {
                setSortType(SORT_TYPES.byCategory);
                handleSortTypeChosen(SORT_TYPES.byCategory, sortDescending);
              }}
            >
              By category
            </MenuItem>
          )}
          {sortByQuantity && (
            <MenuItem
              value="quantity"
              key={SORT_TYPES.byQuantity}
              tabIndex={SORT_TYPES.byQuantity}
              selected={sortType === SORT_TYPES.byQuantity}
              onClick={() => {
                setSortType(SORT_TYPES.byQuantity);
                handleSortTypeChosen(SORT_TYPES.byQuantity, sortDescending);
              }}
            >
              By quantity
            </MenuItem>
          )}
        </Menu>
      </AppBar>
    </Slide>
  );
};

export default PantryAppBar;
