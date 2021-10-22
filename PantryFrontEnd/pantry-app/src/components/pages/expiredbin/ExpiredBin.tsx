import {
  AppBar,
  Avatar,
  Box,
  Button,
  ButtonGroup,
  Card,
  Container,
  createStyles,
  CssBaseline,
  IconButton,
  List,
  ListItem,
  ListItemAvatar,
  ListItemSecondaryAction,
  ListItemText,
  makeStyles,
  TextField,
  Theme,
  Toolbar,
  Typography,
  useTheme,
} from "@material-ui/core";
import {
  AddCircle,
  AllInclusiveOutlined,
  CheckOutlined,
  DeleteOutlined,
  LibraryBooksOutlined,
  RemoveCircle,
} from "@material-ui/icons";
import InfoIcon from "@material-ui/icons/Info";
import SearchIcon from "@material-ui/icons/SearchOutlined";
import FilterIcon from "@material-ui/icons/TuneOutlined";
import React from "react";

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    container: {
      height: "100vh",
      marginLeft: theme.spacing(35),
      backgroundColor: theme.palette.background.default,
      padding: theme.spacing(4),
    },
    root: {
      right: 0,
      top: 0,
      width: theme.spacing(6),
      borderWidth: 10,
      transition: "width 250ms ease-in-out",
      float: "right",
      "&.Mui-focused": {
        width: theme.spacing(50),
        float: "right",
      },
    },
    appBar: {
      padding: 10,
      display: "flex",
      flexDirection: "row",
      justifyContent: "flex-end",
      alignItems: "center",
      width: `calc(100vw - ${theme.spacing(35)}px)`,
    },
    title: {
      fontSize: "2rem",
      alignSelf: "center",
      marginRight: "auto",
      marginLeft: theme.spacing(4),
      fontWeight: "bolder",
    },
    incDecGroup: {
      backgroundColor: theme.palette.primary.light,
    },
    buttonCounter: {
      fontSize: "1.25rem",
      fontWeight: 600,
      color: theme.palette.text.primary,
      opacity: 1,
    },
    disabledDec: {
      "&.Mui-disabled": {
        backgroundColor: theme.palette.text.secondary,
      },
    },
    checkBtn: {
      backgroundColor: theme.palette.primary.main,
      "&:hover": {
        backgroundColor: theme.palette.primary.light,
      },
    },
    btnIcons: {
      color: theme.palette.background.default,
    },
    removeBtn: {
      backgroundColor: theme.palette.error.main,
      "&:hover": {
        backgroundColor: theme.palette.error.light,
        opacity: 0.2,
      },
    },
    recipeBtn: {
      backgroundColor: theme.palette.secondary.main,
    },
    checked: {
      opacity: 0.5,
    },
    style: {
      background: "#FFFFFF",
    },
  })
);

const ExpiredBin: React.FC = () => {
  const theme = useTheme();
  const classes = useStyles(theme);

  const [expanded, setExpanded] = React.useState(false);

  const [count, setCount] = React.useState(1);

  const [checked, setChecked] = React.useState(false);

  return (
    <div className={classes.container}>
      <AppBar
        color="transparent"
        className={classes.appBar}
        classes={{ root: classes.appBar }}
      >
        <Typography variant="h2" color="textPrimary" className={classes.title}>
          Expired Items
        </Typography>

        <TextField
          placeholder="Search for an item in shopping list"
          onFocus={() => setExpanded(true)}
          onBlur={() => setExpanded(false)}
          size="medium"
          color="primary"
          variant="outlined"
          InputProps={{
            classes: {
              focused: classes.root,
              root: classes.root,
            },
            endAdornment: expanded ? (
              <IconButton>
                <SearchIcon />
              </IconButton>
            ) : (
              <SearchIcon
                onClick={(e) => {
                  e.currentTarget.parentElement
                    ? e.currentTarget.parentElement.click()
                    : console.log("parent null");
                }}
              />
            ),
          }}
        />
        <IconButton>
          <FilterIcon />
        </IconButton>
      </AppBar>

      <Toolbar />

      <Container>
        <List>
          <Card className={checked ? classes.checked : undefined}>
            <ListItem>
              <ListItemAvatar>
                <Avatar variant="rounded">
                  <AllInclusiveOutlined />
                </Avatar>
              </ListItemAvatar>
              <ListItemText
                primary={
                  <Box
                    display="flex"
                    justifyContent="flex-start"
                    alignItems="center"
                    minHeight={0}
                  >
                    <CssBaseline />
                    <Typography variant="h6">Human food item 1</Typography>
                    <IconButton>
                      <InfoIcon color="primary" />
                    </IconButton>
                  </Box>
                }
                secondary="Expired on: 30/02/2022"
                secondaryTypographyProps={{
                  variant: "body2",
                  color: "error",
                }}
              ></ListItemText>
              <Box
                component={ListItemSecondaryAction}
                flex
                justifyContent="space-around"
              >
                <ButtonGroup
                  variant="outlined"
                  classes={{ groupedOutlinedHorizontal: classes.incDecGroup }}
                  color={"default"}
                >
                  <IconButton
                    color="primary"
                    disabled={count === 1}
                    onClick={() => setCount((prevCount) => prevCount - 1)}
                    classes={{ disabled: classes.disabledDec }}
                  >
                    <RemoveCircle />
                  </IconButton>
                  <Button
                    disabled
                    variant="text"
                    classes={{
                      text: classes.buttonCounter,
                    }}
                  >
                    {count}
                  </Button>
                  <IconButton
                    color="primary"
                    onClick={() => setCount((prevCount) => prevCount + 1)}
                  >
                    <AddCircle />
                  </IconButton>
                </ButtonGroup>
                <Box ml={theme.spacing(5)} component={ButtonGroup}>
                  <Button
                    variant="contained"
                    size="large"
                    disableElevation
                    classes={{ root: classes.removeBtn }}
                  >
                    <DeleteOutlined classes={{ root: classes.btnIcons }} />
                  </Button>
                  <Button
                    variant="contained"
                    size="large"
                    disableElevation
                    classes={{ root: classes.recipeBtn }}
                  >
                    <LibraryBooksOutlined
                      classes={{ root: classes.btnIcons }}
                    />
                  </Button>
                  <Button
                    variant="contained"
                    size="large"
                    disableElevation
                    classes={{ root: classes.checkBtn }}
                    onClick={() => {
                      setChecked((prevChecked) => !prevChecked);
                    }}
                  >
                    <CheckOutlined classes={{ root: classes.btnIcons }} />
                  </Button>
                </Box>
              </Box>
            </ListItem>
          </Card>

          <Card className={checked ? classes.checked : undefined}>
            <ListItem>
              <ListItemAvatar>
                <Avatar variant="rounded">
                  <AllInclusiveOutlined />
                </Avatar>
              </ListItemAvatar>
              <ListItemText
                primary={
                  <Box
                    display="flex"
                    justifyContent="flex-start"
                    alignItems="center"
                    minHeight={0}
                  >
                    <CssBaseline />
                    <Typography variant="h6">Human food item 2</Typography>
                    <IconButton>
                      <InfoIcon color="primary" />
                    </IconButton>
                  </Box>
                }
                secondary="Expired on: 06/03/2022"
                secondaryTypographyProps={{
                  variant: "body2",
                  color: "error",
                }}
              ></ListItemText>
              <Box
                component={ListItemSecondaryAction}
                flex
                justifyContent="space-around"
              >
                <ButtonGroup
                  variant="outlined"
                  classes={{ groupedOutlinedHorizontal: classes.incDecGroup }}
                  color={"default"}
                >
                  <IconButton
                    color="primary"
                    disabled={count === 1}
                    onClick={() => setCount((prevCount) => prevCount - 1)}
                    classes={{ disabled: classes.disabledDec }}
                  >
                    <RemoveCircle />
                  </IconButton>
                  <Button
                    disabled
                    variant="text"
                    classes={{
                      text: classes.buttonCounter,
                    }}
                  >
                    {count}
                  </Button>
                  <IconButton
                    color="primary"
                    onClick={() => setCount((prevCount) => prevCount + 1)}
                  >
                    <AddCircle />
                  </IconButton>
                </ButtonGroup>
                <Box ml={theme.spacing(5)} component={ButtonGroup}>
                  <Button
                    variant="contained"
                    size="large"
                    disableElevation
                    classes={{ root: classes.removeBtn }}
                  >
                    <DeleteOutlined classes={{ root: classes.btnIcons }} />
                  </Button>
                  <Button
                    variant="contained"
                    size="large"
                    disableElevation
                    classes={{ root: classes.recipeBtn }}
                  >
                    <LibraryBooksOutlined
                      classes={{ root: classes.btnIcons }}
                    />
                  </Button>
                  <Button
                    variant="contained"
                    size="large"
                    disableElevation
                    classes={{ root: classes.checkBtn }}
                    onClick={() => {
                      setChecked((prevChecked) => !prevChecked);
                    }}
                  >
                    <CheckOutlined classes={{ root: classes.btnIcons }} />
                  </Button>
                </Box>
              </Box>
            </ListItem>
          </Card>

          <Card className={checked ? classes.checked : undefined}>
            <ListItem>
              <ListItemAvatar>
                <Avatar variant="rounded">
                  <AllInclusiveOutlined />
                </Avatar>
              </ListItemAvatar>
              <ListItemText
                primary={
                  <Box
                    display="flex"
                    justifyContent="flex-start"
                    alignItems="center"
                    minHeight={0}
                  >
                    <CssBaseline />
                    <Typography variant="h6">Human food item 3</Typography>
                    <IconButton>
                      <InfoIcon color="primary" />
                    </IconButton>
                  </Box>
                }
                secondary="Expired on: 30/03/2022"
                secondaryTypographyProps={{
                  variant: "body2",
                  color: "error",
                }}
              ></ListItemText>
              <Box
                component={ListItemSecondaryAction}
                flex
                justifyContent="space-around"
              >
                <ButtonGroup
                  variant="outlined"
                  classes={{ groupedOutlinedHorizontal: classes.incDecGroup }}
                  color={"default"}
                >
                  <IconButton
                    color="primary"
                    disabled={count === 1}
                    onClick={() => setCount((prevCount) => prevCount - 1)}
                    classes={{ disabled: classes.disabledDec }}
                  >
                    <RemoveCircle />
                  </IconButton>
                  <Button
                    disabled
                    variant="text"
                    classes={{
                      text: classes.buttonCounter,
                    }}
                  >
                    {count}
                  </Button>
                  <IconButton
                    color="primary"
                    onClick={() => setCount((prevCount) => prevCount + 1)}
                  >
                    <AddCircle />
                  </IconButton>
                </ButtonGroup>
                <Box ml={theme.spacing(5)} component={ButtonGroup}>
                  <Button
                    variant="contained"
                    size="large"
                    disableElevation
                    classes={{ root: classes.removeBtn }}
                  >
                    <DeleteOutlined classes={{ root: classes.btnIcons }} />
                  </Button>
                  <Button
                    variant="contained"
                    size="large"
                    disableElevation
                    classes={{ root: classes.recipeBtn }}
                  >
                    <LibraryBooksOutlined
                      classes={{ root: classes.btnIcons }}
                    />
                  </Button>
                  <Button
                    variant="contained"
                    size="large"
                    disableElevation
                    classes={{ root: classes.checkBtn }}
                    onClick={() => {
                      setChecked((prevChecked) => !prevChecked);
                    }}
                  >
                    <CheckOutlined classes={{ root: classes.btnIcons }} />
                  </Button>
                </Box>
              </Box>
            </ListItem>
          </Card>
        </List>
      </Container>
    </div>
  );
};

export default ExpiredBin;
