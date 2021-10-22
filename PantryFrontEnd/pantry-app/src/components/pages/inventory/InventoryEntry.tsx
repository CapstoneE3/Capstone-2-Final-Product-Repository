import {
  Avatar,
  Card,
  Collapse,
  Container,
  CssBaseline,
  IconButton,
  LinearProgress,
  ListItem,
  ListItemAvatar,
  ListItemIcon,
  ListItemText,
  SvgIconTypeMap,
  Theme,
  Typography,
  useTheme,
  Button,
  Hidden,
  ButtonGroup,
  Grid,
  Box,
} from "@material-ui/core";
import { OverridableComponent } from "@material-ui/core/OverridableComponent";
import {
  KeyboardArrowDownOutlined,
  Info as InfoIcon,
  CheckOutlined,
} from "@material-ui/icons";
import { createStyles, makeStyles } from "@material-ui/styles";
import { useSnackbar } from "notistack";
import React from "react";
import { DOMAIN } from "../../../App";
import { ExpiryGroup } from "./mockEntries";

interface EntryProps {
  FoodIcon?: OverridableComponent<SvgIconTypeMap<{}, "svg">>;
  inExpiryGroups?: ExpiryGroup[];
  name?: string;
  category?: string;
  quantity?: string;
  itemID: string;
  photo?: string;
}

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    expandEntryButton: {
      transition: "transform 250ms ease-in-out",
    },
    mainTextContainer: {
      display: "flex",
      justifyContent: "flex-start",
      alignItems: "center",
      padding: 0,
      paddingLeft: theme.spacing(1),
    },
    embeddedMainTextContainer: {
      padding: 0,
      margin: 0,
      minWidth: 0,
      width: "fit-content",
    },
    secondaryTextContainer: {
      paddingRight: 0,
      paddingLeft: theme.spacing(1),
    },
    entryAvatarRoot: {
      height: "100%",
      aspectRatio: "1 / 1",
      minWidth: theme.spacing(16),
      [theme.breakpoints.down("xs")]: {
        minWidth: theme.spacing(10),
      },
    },
    h5Down: {
      [theme.breakpoints.down("sm")]: {
        fontSize: theme.typography.h6.fontSize,
      },
    },
    h6Down: {
      [theme.breakpoints.down("sm")]: {
        fontSize: theme.typography.body1.fontSize,
      },
    },
    h2Down: {
      [theme.breakpoints.down("sm")]: {
        fontSize: theme.typography.h5.fontSize,
      },
    },
    body1Down: {
      [theme.breakpoints.down("sm")]: {
        fontSize: theme.typography.body2.fontSize,
      },
    },
    infoButton: {
      // color: "#97A7E5",
      color: "#AD9FCE",
      [theme.breakpoints.down("sm")]: {
        marginBottom: 0,
      },
    },
    infoExpandButtonGroup: {
      maginLeft: theme.spacing(2),
      marginBottom: theme.spacing(9),
      [theme.breakpoints.down("xs")]: {
        marginLeft: "auto",
        marginBottom: 0,
      },
    },
    collapseRoot: {
      backgroundColor: theme.palette.primary.light,
      paddingTop: theme.spacing(2),
      paddingBottom: theme.spacing(2),
    },
    collapseRootHidden: {
      paddingTop: 0,
      paddingBottom: 0,
    },
    expiryGroupContainer: {
      display: "flex",
      flexDirection: "row",
      justifyContent: "space-between",
      alignItems: "center",
      paddingTop: theme.spacing(2),
      paddingBottom: theme.spacing(2),
    },
    expiryRemainTyp: {
      display: "inline",
      marginLeft: theme.spacing(2),
      [theme.breakpoints.down("xs")]: {
        marginLeft: 0,
        display: "block",
      },
    },

    expired: {
      backgroundColor: theme.palette.error.main,
      color: theme.palette.error.main,
    },
    expiring: {
      backgroundColor: theme.palette.secondary.main,
      color: theme.palette.secondary.main,
    },
    notExpiring: {
      backgroundColor: theme.palette.primary.main,
      color: theme.palette.primary.main,
    },
  })
);

const today = new Date();
const msToDaysRatio = 1000 * 60 * 60 * 24;
const msToMonthsRatio = msToDaysRatio * 30;

const InventoryEntry: React.FC<EntryProps> = ({
  FoodIcon = InfoIcon,
  inExpiryGroups = [],
  name = "Human food item 1",
  category = "Human food",
  quantity = "1kg",
  photo,
  itemID,
}) => {
  const theme = useTheme();
  const classes = useStyles(theme);
  const [isOpen, setOpen] = React.useState(false);
  const [earliestExpPc, setEarliestExpPc] = React.useState<number>(100);
  const [earliestExpDate, setEarliestExpDate] = React.useState("01/01/1970");
  const [expRemainStr, setRemainStr] = React.useState("12+ months");
  const [colorClass, setColorClass] = React.useState(classes.notExpiring);
  const [expiryGroups, setExpiryGroups] = React.useState(inExpiryGroups);
  const [count, setCount] = React.useState(0);
  const { enqueueSnackbar } = useSnackbar();

  React.useEffect(() => {
    setCount(expiryGroups.reduce((prev, curr) => prev + curr.count, 0));
  }, [expiryGroups]);

  const handleNewEarliestExpiry = (
    expDate: string,
    percentageDiff: number,
    diffStr: string
  ) => {
    setEarliestExpDate(expDate);
    setEarliestExpPc(percentageDiff);
    setRemainStr(diffStr);
    if (percentageDiff <= 0) setColorClass(classes.expired);
    else if (percentageDiff < 5) setColorClass(classes.expiring);
  };

  const handleDeleteItem = React.useCallback(
    (index: number) => {
      setExpiryGroups((prev) => {
        let newExpGroups = [...prev];
        let currGroup = newExpGroups[index];

        if (currGroup) {
          currGroup.count--;
          fetch(DOMAIN + "/api/removeInventoryItem", {
            method: "DELETE",
            credentials: "include",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
              productID: itemID,
              exp: currGroup.expDate.toLocaleDateString(),
              count: 1,
            }),
          })
            .then((resp) => resp.json())
            .then((data) => {
              enqueueSnackbar(data.message, { variant: "success" });
            })
            .catch((e) => enqueueSnackbar("Error! " + e, { variant: "error" }));
        }
        if (currGroup && currGroup.count <= 0) newExpGroups.splice(index, 1);

        return newExpGroups;
      });
    },
    [enqueueSnackbar, itemID]
  );
  return (
    <>
      {count > 0 && (
        <Card
          elevation={3}
          style={{
            marginTop: theme.spacing(2),
            marginBottom: theme.spacing(2),
          }}
        >
          <ListItem component="li">
            <ListItemAvatar
              classes={{
                root: classes.entryAvatarRoot,
              }}
            >
              <Avatar
                variant="rounded"
                style={{ width: "100%", height: "100%" }}
              >
                {photo ? (
                  <img
                    src={`https://shop.coles.com.au${photo}`}
                    alt="recipe"
                    width={`${theme.spacing(16)}`}
                  />
                ) : (
                  <FoodIcon />
                )}
              </Avatar>
            </ListItemAvatar>
            <ListItemText
              primary={
                <Container classes={{ root: classes.mainTextContainer }}>
                  <CssBaseline />
                  <Container
                    classes={{ root: classes.embeddedMainTextContainer }}
                  >
                    <Typography
                      variant="h6"
                      // noWrap
                      classes={{ root: classes.h6Down }}
                      display="block"
                    >
                      {name}
                    </Typography>
                    <Typography
                      paragraph={false}
                      color="textSecondary"
                      display="inline"
                      classes={{ root: classes.body1Down }}
                    >
                      {category}
                    </Typography>
                    <Typography
                      // noWrap
                      variant="h5"
                      display="block"
                      color="textSecondary"
                      // style={{ marginLeft: theme.spacing(1) }}
                      classes={{ root: classes.h5Down }}
                    >
                      {quantity} x {count}
                    </Typography>
                  </Container>
                  <ButtonGroup
                    orientation="vertical"
                    variant="outlined"
                    className={classes.infoExpandButtonGroup}
                  >
                    <Hidden>
                      <IconButton className={classes.infoButton}>
                        <InfoIcon
                          color="primary"
                          fontSize="medium"
                          className={classes.infoButton}
                        />
                      </IconButton>
                    </Hidden>
                    <Hidden smUp>
                      <IconButton
                        onClick={() => setOpen((prevOpen) => !prevOpen)}
                        color="primary"
                      >
                        <KeyboardArrowDownOutlined
                          fontSize="medium"
                          classes={{ root: classes.expandEntryButton }}
                          style={isOpen ? { transform: "rotate(180deg)" } : {}}
                        />
                      </IconButton>
                    </Hidden>
                  </ButtonGroup>
                </Container>
              }
              secondary={
                <Container classes={{ root: classes.secondaryTextContainer }}>
                  <Typography
                    variant="body1"
                    paragraph={false}
                    display="inline"
                    classes={{ root: classes.body1Down }}
                    color="textPrimary"
                  >
                    Earliest expiry:
                  </Typography>
                  <Typography
                    // color={earliestExpPc < 50 ? "secondary" : "primary"}
                    display="inline"
                    style={{
                      marginLeft: theme.spacing(1),
                      backgroundColor: "transparent",
                    }}
                    // className={colorClass}
                    classes={{ root: colorClass }}
                  >
                    {earliestExpDate}
                  </Typography>
                  <Typography
                    color="textSecondary"
                    variant="body1"
                    // style={{ marginLeft: theme.spacing(2) }}
                    className={classes.expiryRemainTyp}
                  >
                    {expRemainStr}
                  </Typography>
                </Container>
              }
            />
            <Hidden mdDown>
              <Container maxWidth="sm" disableGutters={true}>
                <LinearProgress
                  variant="determinate"
                  value={100 - earliestExpPc}
                  color={earliestExpPc < 5 ? "secondary" : "primary"}
                  // color={exp}
                  classes={{ bar1Determinate: colorClass }}
                />
              </Container>
            </Hidden>
            <Hidden xsDown>
              <ListItemIcon>
                <IconButton
                  onClick={() => setOpen((prevOpen) => !prevOpen)}
                  color="primary"
                >
                  <KeyboardArrowDownOutlined
                    fontSize="medium"
                    classes={{ root: classes.expandEntryButton }}
                    style={isOpen ? { transform: "rotate(180deg)" } : {}}
                  />
                </IconButton>
              </ListItemIcon>
            </Hidden>
          </ListItem>
          <Hidden lgUp>
            <Container disableGutters={true}>
              <LinearProgress
                variant="determinate"
                value={100 - earliestExpPc}
                color={earliestExpPc < 5 ? "secondary" : "primary"}
                classes={{ bar1Determinate: colorClass }}
              />
            </Container>
          </Hidden>
          <Collapse
            in={isOpen}
            className={classes.collapseRoot}
            classes={{ hidden: classes.collapseRootHidden }}
          >
            <Grid
              container
              justifyContent="space-between"
              alignItems="center"
              spacing={1}
            >
              {expiryGroups.map((eg, i) => {
                eg.expDate = new Date(eg.expDate);
                const diff = getMonthDifference(eg.expDate);
                const localDateStr = eg.expDate.toLocaleDateString();

                var percentageDiff;
                var color = classes.notExpiring;
                var diffStr =
                  diff <= 1
                    ? "" + Math.floor(diff * 31) + " days"
                    : "" + Math.floor(diff) + " months";

                if (diff > 12) {
                  percentageDiff = 100;
                  diffStr = "12+ months";
                } else {
                  percentageDiff = (diff / 12) * 100;
                }

                if (percentageDiff < earliestExpPc) {
                  handleNewEarliestExpiry(
                    localDateStr,
                    percentageDiff,
                    diffStr
                  );
                }

                if (percentageDiff < 0) color = classes.expired;
                else if (percentageDiff < 5) color = classes.expiring;

                return (
                  <Grid item xs={12} lg={6} key={i}>
                    <Container>
                      <Card
                        elevation={2}
                        style={{ opacity: percentageDiff <= 0 ? 0.5 : 1 }}
                      >
                        <Container className={classes.expiryGroupContainer}>
                          {/* {localDateStr} x {eg.count} ({diffStr}) */}
                          <Box flex>
                            <Typography variant="h6">{localDateStr}</Typography>
                            <Typography variant="h6" color="textSecondary">
                              ({diffStr})
                            </Typography>
                          </Box>
                          <Typography variant="h6" color="textPrimary">
                            x {eg.count}
                          </Typography>
                          <ButtonGroup
                            variant="contained"
                            size="large"
                            disableElevation
                          >
                            <Button
                              color="primary"
                              onClick={() => handleDeleteItem(i)}
                            >
                              <CheckOutlined style={{ color: "white" }} />
                            </Button>
                            {/* <Button
                            style={{
                              backgroundColor: theme.palette.error.main,
                            }}
                          >
                            <DeleteOutlined style={{ color: "white" }} />
                          </Button> */}
                          </ButtonGroup>
                        </Container>
                        <LinearProgress
                          variant="determinate"
                          value={100 - percentageDiff}
                          color={
                            color === classes.expiring ? "secondary" : "primary"
                          }
                          // color={"error"}
                          classes={{
                            bar1Determinate: color,
                          }}
                        />
                      </Card>
                    </Container>
                  </Grid>
                );
              })}
            </Grid>
          </Collapse>
        </Card>
      )}
    </>
  );
};

export const getMonthDifference = (expDate: Date) => {
  const msDiff = Math.floor(expDate.getTime() - today.getTime());

  return msDiff / msToMonthsRatio;
};

export default InventoryEntry;
