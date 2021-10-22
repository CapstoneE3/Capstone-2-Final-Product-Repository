import {
  AppBar,
  Card,
  CardContent,
  CardHeader,
  Container,
  Grid,
  IconButton,
  makeStyles,
  TextField,
  Theme,
  Toolbar,
  Typography,
  useTheme,
} from "@material-ui/core";
import { createStyles } from "@material-ui/core/styles";
import SearchIcon from "@material-ui/icons/SearchOutlined";
import FilterIcon from "@material-ui/icons/TuneOutlined";
import React from "react";

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    container: {
      width: "auto",
      height: "100vh",
      backgroundColor: theme.palette.text.secondary,
      padding: "2rem",
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
    style: {
      background: "#FFFFFF",
    },
    h3: {
      color: "#2E3B55",
    },
    flexContainer: {
      display: "flex",
      flexDirection: "row",
      padding: 0,
    },
  })
);

interface AccountsProps {}

const Account: React.FC<AccountsProps> = () => {
  const theme = useTheme();
  const classes = useStyles(theme);

  const [expanded, setExpanded] = React.useState(false);

  return (
    <div className={classes.container}>
      <React.Fragment>
        <AppBar
          color="transparent"
          className={classes.appBar}
          classes={{ root: classes.appBar }}
        >
          <Typography
            variant="h2"
            color="textPrimary"
            className={classes.title}
          >
            Account
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
          <Grid container spacing={1} item xs={10}>
            <Grid item xs={4} spacing={3} md={3}>
              <Card>
                <CardHeader
                  action={<IconButton></IconButton>}
                  title="Finished Products"
                />
                <CardContent></CardContent>
              </Card>
            </Grid>
            <Grid item xs={12} spacing={2} md={3}>
              <Card>
                <CardHeader
                  action={<IconButton></IconButton>}
                  title="Food Wasted"
                />
                <CardContent></CardContent>
              </Card>
            </Grid>
          </Grid>
        </Container>
      </React.Fragment>
    </div>
  );
};
export default Account;
