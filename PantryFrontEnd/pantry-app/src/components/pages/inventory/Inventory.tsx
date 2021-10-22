import {
  Container,
  Divider,
  makeStyles,
  Tab,
  Tabs,
  Theme,
  Toolbar,
  useTheme,
} from "@material-ui/core";
import { createStyles } from "@material-ui/core/styles";
import InfoIcon from "@material-ui/icons/Info";
import React, { ReactNode } from "react";
import { GiFruitBowl as FruitsIcon } from "react-icons/gi";
import SwipeableViews from "react-swipeable-views";
import { DOMAIN } from "../../../App";
import PantryAppBar, { SORT_TYPES } from "../../PantryAppBar";
import InventoryTab from "./InventoryTab";
import { APIItem, Item } from "./mockEntries";

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    // backgroundColor: "#f4f2f8",
    // backgroundColor: "#dfedf5",
    container: {
      marginLeft: theme.spacing(30),
      paddingTop: theme.spacing(2),
      height: "auto",
      minHeight: "100vh",
      backgroundColor: "#f1f9f3",
      [theme.breakpoints.down("sm")]: {
        marginLeft: 0,
      },
    },
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
    appBar: {
      padding: 10,
      display: "flex",
      flexDirection: "row",
      justifyContent: "flex-end",
      alignItems: "center",
      // width: `calc(100vw - ${theme.spacing(30)}px)`,
      width: "auto",
      left: theme.spacing(30),
      backgroundColor: theme.palette.background.default,
      borderBottom: "3px solid " + theme.palette.primary.dark,
      [theme.breakpoints.down("sm")]: {
        // width: "100vw",
        left: 0,
      },
    },
    title: {
      fontSize: "2rem",
      alignSelf: "center",
      marginRight: "auto",
      marginLeft: theme.spacing(4),
      fontWeight: "bolder",
      letterSpacing: 3,
    },
    tabBar: {
      color: theme.palette.primary.dark,
      backgroundColor: theme.palette.primary.light,
      width: "100%",
      position: "sticky",
      zIndex: 10,
      top: 0,
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
  })
);

interface InventoryProps {
  setNavOpen: () => void;
}

const Inventory: React.FC<InventoryProps> = ({ setNavOpen }) => {
  const theme = useTheme();
  const classes = useStyles(theme);

  const getInitialEntries: () => void = () => {
    const params: RequestInit = {
      method: "GET",
      credentials: "include",
      headers: {
        "Content-Type": "application/json",
      },
    };

    fetch(DOMAIN + "/api/GetInventoryList", params)
      .then((resp) => resp.json())
      .then((data) => {
        console.log(data);
        let entries: Item[] = [];

        Object.keys(data).forEach((category) => {
          const products: APIItem[] = data[category];

          for (let product of products) {
            entries.push({ ...product, category: category });
          }
        });
        console.log(entries);
        setEntries(sortByExpiry(entries, true));
      });
  };

  React.useEffect(() => {
    getInitialEntries();
  }, []);

  const getTabCategories: () => string[] = () => {
    //need to fetch first in reality

    let allCategories: string[] = [];

    entries.forEach((item) => {
      if (!allCategories.includes(item.category)) {
        allCategories.push(item.category);
      }
    });

    return allCategories;
  };

  const [activeTab, setActiveTab] = React.useState(0);

  const [entries, setEntries] = React.useState<Item[]>(() => []);

  const [searchEntries, setSearchEntries] = React.useState<Item[]>([]);

  const tabCategories = React.useMemo(getTabCategories, [entries]);

  const topEleRef = React.useRef<HTMLDivElement | null>(null);
  const handleTabChange = (e: React.ChangeEvent<{}>, newTab: number) => {
    setActiveTab(newTab);
  };

  const getEntriesForCategory = React.useCallback(
    (category: string | undefined) => {
      if (category) {
        return entries.filter((item) => {
          return item.category === category ? true : false;
        });
      }
      return entries;
    },
    [entries]
  );

  const filterSearchEntries = React.useCallback(
    (searchTerm: string) => {
      return entries.filter((entry) => {
        searchTerm = searchTerm.toLowerCase();
        return entry.name.toLowerCase().includes(searchTerm) ||
          entry.category.toLowerCase().includes(searchTerm)
          ? true
          : false;
      });
    },
    [entries]
  );

  const handleOpenMenu = () => {
    setNavOpen();
  };

  const handleSearchClick = React.useCallback(
    (searchTerm: string) => {
      if (searchTerm.length > 0) {
        setSearchEntries(filterSearchEntries(searchTerm));
        setActiveTab(tabCategories.length + 1);
      }
    },
    [filterSearchEntries, tabCategories.length]
  );

  const handleSortDirectionChange = (sortType: number, desc: boolean) => {
    console.log(desc);
    handleSortTypeChosen(sortType, desc);
  };

  const handleSortTypeChosen = React.useCallback(
    (sortBy: number, desc: boolean) => {
      switch (sortBy) {
        case SORT_TYPES.byExpiryDate:
          setEntries((prevEntries) => [...sortByExpiry(prevEntries, desc)]);
          break;
        default:
        case SORT_TYPES.byName:
          setEntries((prevEntries) => [...sortByName(prevEntries, desc)]);
          break;
        case SORT_TYPES.byCategory:
          setEntries((prevEntries) => [...sortByCategory(prevEntries, desc)]);
          break;
        case SORT_TYPES.byQuantity:
          setEntries((prevEntries) => [...sortByQuantity(prevEntries, desc)]);
          break;
      }
    },
    []
  );

  const getTabs = () => {
    return tabCategories.map((tab, i) => {
      return (
        <Tab
          wrapped
          key={tab}
          label={tab}
          value={i + 1}
          icon={<FruitsIcon size={29} />}
        />
      );
    });
  };

  const tabs = React.useMemo(getTabs, [tabCategories]);

  const memoizedTabs = React.useMemo(() => {
    return tabCategories.map((category, index) => {
      const filteredEntries = getEntriesForCategory(category);
      return (
        <InventoryTab
          key={index + 1}
          activeTab={activeTab}
          index={index + 1}
          propEntries={filteredEntries}
          category={category}
        />
      );
    });
  }, [activeTab, tabCategories, getEntriesForCategory]);

  return (
    <div className={classes.container}>
      <div ref={topEleRef} />
      <PantryAppBar
        title={"Inventory"}
        handleOpenMenu={handleOpenMenu}
        handleSearchClick={handleSearchClick}
        handleSortDirectionChange={handleSortDirectionChange}
        handleSortTypeChosen={handleSortTypeChosen}
        sortByCategory
        sortByExpiry
        sortByName
        sortByQuantity
      />
      <Toolbar />

      <Container disableGutters style={{ maxWidth: "none" }}>
        <Tabs
          value={activeTab}
          variant="scrollable"
          scrollButtons="on"
          onChange={handleTabChange}
          classes={{ root: classes.tabBar }}
        >
          <Tab wrapped label={"All"} value={0} icon={<InfoIcon />} />
          {tabs}
          <Tab
            wrapped
            label={"Search"}
            value={tabCategories.length + 1}
            icon={<InfoIcon />}
          />
        </Tabs>

        <Divider color="primary" />
        <SwipeableViews
          index={activeTab}
          onChangeIndex={(index) => setActiveTab(index)}
        >
          <InventoryTab activeTab={activeTab} index={0} propEntries={entries} />
          {memoizedTabs}
          <InventoryTab
            activeTab={activeTab}
            index={tabCategories.length + 1}
            propEntries={searchEntries}
          />
        </SwipeableViews>
      </Container>
    </div>
  );
};

const areEqual: (
  prevProps: Readonly<InventoryProps & { children?: ReactNode }>,
  nextProps: Readonly<InventoryProps & { children?: ReactNode }>
) => boolean = (prevProps, nextProps) => {
  console.log("compared functions");
  return true;
};

const sortByExpiry = (entries: Item[], desc: boolean) => {
  const sorted = entries.sort((entryA, entryB) => {
    const expGrpsA = entryA.expiry_Count;
    const expGrpsB = entryB.expiry_Count;

    const earliestExpA = expGrpsA.reduce((prev, curr) =>
      prev.expDate < curr.expDate ? prev : curr
    );

    const earliestExpB = expGrpsB.reduce((prev, curr) =>
      prev.expDate < curr.expDate ? prev : curr
    );

    if (desc) return earliestExpA.expDate < earliestExpB.expDate ? -1 : 1;
    return earliestExpA.expDate < earliestExpB.expDate ? 1 : -1;
  });

  return sorted;
};

const sortByName = (entries: Item[], desc: boolean) => {
  const sorted = entries.sort((A, B) =>
    desc ? A.name.localeCompare(B.name) : A.name.localeCompare(B.name) * -1
  );

  return sorted;
};

const sortByCategory = (entries: Item[], desc: boolean) => {
  const sorted = entries.sort((A, B) =>
    desc
      ? A.category.localeCompare(B.category)
      : A.category.localeCompare(B.category) * -1
  );

  return sorted;
};

const sortByQuantity = (entries: Item[], desc: boolean) => {
  const sorted = entries.sort((A, B) => {
    const expGrpsA = A.expiry_Count;
    const expGrpsB = B.expiry_Count;

    var totalCountA = 0;
    expGrpsA.forEach((eg) => (totalCountA += eg.count));

    var totalCountB = 0;
    expGrpsB.forEach((eg) => (totalCountB += eg.count));

    const diff = totalCountA - totalCountB;

    return desc ? diff : diff * -1;
  });

  return sorted;
};

const MemoizedInventory = React.memo(Inventory, areEqual);
export default MemoizedInventory;
