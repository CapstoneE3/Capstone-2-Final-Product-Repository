import {
  CircularProgress,
  CssBaseline,
  Hidden,
  Typography,
} from "@material-ui/core";
import { createTheme, ThemeProvider } from "@material-ui/core/styles";
import { LocationDescriptor } from "history";
import { useSnackbar } from "notistack";
import React from "react";
import { BrowserRouter, Redirect, Route, Switch } from "react-router-dom";
import NavBar, { navItem, navItems } from "./components/navbar/NavBar";
import Login from "./components/pages/login/Login";
import SignUp from "./components/pages/signup/Signup";
import { pageToIndex } from "./components/routingTable";

const theme = createTheme({
  palette: {
    primary: {
      // dark: "#31C15B",
      // main: "#52D378",
      // dark: "#52D378",
      // main: "#31C15B",
      dark: "#2aa64e",
      main: "#2eb455",
      light: "#BDF2C9",
    },
    secondary: {
      // main: "#efa448",
      // main: "#e88b65",
      light: "#F9DFAF",
      main: "#f08559",
      dark: "#e4794c",
    },
    text: {
      primary: "#666666",
      secondary: "#BDBDBD",
      // secondary: "#edabeC",
    },
    background: {
      default: "#FFFFFF",
      // default: "#f4f2f8",
    },
    info: {
      main: "#a2e9f3",
    },
    error: {
      main: "#d24d43",
    },
  },
  spacing: 8,
  typography: {
    fontFamily: "Cambay,sans-serif",
    fontSize: 16,
  },
});

export const DOMAIN = "https://pantties.azurewebsites.net";

interface SetLoggedIn {
  setLoggedIn: React.Dispatch<React.SetStateAction<boolean>>;
}
export type MyLocationDesc = LocationDescriptor<any> & SetLoggedIn;

function App() {
  const [navTab, setNavTab] = React.useState(0);

  const loggedInState = React.useState(() => false);

  const [isLoggedIn, setLoggedIn] = loggedInState;

  const [navIsOpen, setNavOpen] = React.useState(false);

  const [isFetching, setFetching] = React.useState(true);

  const callBackNavOpen = React.useCallback(() => {
    setNavOpen((prev) => !prev);
  }, []);

  const { enqueueSnackbar } = useSnackbar();

  const checkLoggedIn = React.useCallback(() => {
    fetch(DOMAIN + "/api/isLoggedIn", {
      method: "GET",
      credentials: "include",
    })
      .then((resp) => resp.json())
      .then((data) => {
        if (data.isLoggedIn) {
          setLoggedIn(true);
          enqueueSnackbar("Welcome back!", { variant: "success" });
        }
        setFetching(false);
      })
      .catch((e) => {
        console.error(e);
        setFetching(false);
      });
  }, [enqueueSnackbar, setLoggedIn]);

  const replaceSubscriptionOnServer = React.useCallback(
    async (subscription: PushSubscription | null) => {
      if (!subscription) return;
      const unsubParams: RequestInit = {
        method: "POST",
        credentials: "include",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(subscription),
      };

      const resp = await fetch(
        DOMAIN + "/api/replaceSubscription",
        unsubParams
      ).catch((e) => {
        console.error("Subscription deletion error:", e);
        enqueueSnackbar("Could not replace subscription from server!", {
          variant: "error",
        });
      });

      if (!resp || !resp.ok) {
        await subscription.unsubscribe();
        enqueueSnackbar("Failed to enable push notifications!", {
          variant: "error",
        });
      } else enqueueSnackbar("Notifications successfully enabled");
    },
    [enqueueSnackbar]
  );

  const syncSubscriptionWithServer = React.useCallback(
    async (subscription: PushSubscription) => {
      const subscriptionParams: RequestInit = {
        method: "POST",
        credentials: "include",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(subscription),
      };

      const resp = await fetch(
        DOMAIN + "/api/subcriptions/",
        subscriptionParams
      ).catch((e) => {
        console.error(e);
        if (subscription) subscription.unsubscribe();
      });

      if (resp && resp.ok) {
        enqueueSnackbar("Notifications successfully enabled!", {
          variant: "success",
        });
      } else if (resp && resp.status === 401) {
        // setTimeout(() => {
        replaceSubscriptionOnServer(subscription);
        // }, 7500);
      } else {
        enqueueSnackbar("Notifications could not be enabled on the server!", {
          variant: "warning",
        });
        if (subscription) subscription.unsubscribe();
        console.error("Subscription unsuccessful:", resp);
      }
    },
    [enqueueSnackbar, replaceSubscriptionOnServer]
  );

  React.useEffect(() => {
    checkLoggedIn();
    fetch(DOMAIN + "/api/Test", {
      method: "GET",
      credentials: "include",
      headers: {
        "Content-Type": "application/json",
      },
    }).then((resp) => (resp.ok ? console.log(resp) : null));
  }, [checkLoggedIn]);

  React.useEffect(() => {
    if (!isLoggedIn) return;
    if (Notification.permission !== "granted")
      enqueueSnackbar("Notifications permissions are needed!", {
        variant: "info",
      });
    Notification.requestPermission((status) => {
      console.log("Permission", status);
    });

    const strForm = process.env["REACT_APP_PUBLIC_VAPID_KEY"];

    // console.log("current key:", strForm);

    if ("serviceWorker" in navigator) {
      navigator.serviceWorker
        .register("/sw.js", { scope: "/" })
        .then(async (value) => {
          const pushSubscriptionOptions: PushSubscriptionOptionsInit = {
            applicationServerKey: strForm,
            userVisibleOnly: true,
          };
          let serviceWorker;

          if (value.installing) serviceWorker = value.installing;
          else if (value.waiting) serviceWorker = value.waiting;
          if (value.active) serviceWorker = value.active;
          if (serviceWorker) {
            serviceWorker.onstatechange = async (e: any) => {
              if (e.target.state === "activated") {
                console.log("activation confirmed");

                try {
                  value.pushManager
                    .subscribe(pushSubscriptionOptions)
                    .then((subscription) => {
                      syncSubscriptionWithServer(subscription);
                    })
                    .catch((e) => {
                      console.error("caught e");
                      throw e;
                    });
                } catch (e) {
                  // a different subscription exists
                  console.log(e);
                  let subscription = await value.pushManager.getSubscription();

                  if (subscription) {
                    const success = await subscription.unsubscribe();
                    if (success) {
                      const newSub = await value.pushManager.subscribe();
                      replaceSubscriptionOnServer(newSub);
                    }
                  }
                }
              }
            };
          }
        });
    }
  }, [
    isLoggedIn,
    enqueueSnackbar,
    syncSubscriptionWithServer,
    replaceSubscriptionOnServer,
  ]);

  const renderRoute = React.useCallback(
    (item: navItem) => {
      const PageComponent = item.component;

      return <PageComponent setNavOpen={callBackNavOpen} />;
    },
    [callBackNavOpen]
  );

  const getNavPageRoutes = () =>
    navItems.map((item: navItem) => {
      return (
        <Route
          key={pageToIndex(item.link)}
          path={item.link}
          render={() => renderRoute(item)}
        />
      );
    });

  const navPageRoutes = React.useMemo(getNavPageRoutes, [renderRoute]);

  const handleLogout = async (callbackFn: () => void) => {
    const resp = await fetch(DOMAIN + "/api/Users/Logout", {
      method: "GET",
      credentials: "include",
      headers: {
        "Content-Type": "application/json",
      },
    }).catch((e) => console.error(e));

    // success
    if (resp && (resp.ok || resp.status === 401)) {
      setLoggedIn(false);
    }
    callbackFn();
  };

  return (
    <div className="App">
      <BrowserRouter>
        <ThemeProvider theme={theme}>
          <CssBaseline />
          {isLoggedIn && (
            <>
              <Hidden smDown={true}>
                <NavBar
                  currNavTab={navTab}
                  drawerVariant="permanent"
                  setNavTab={setNavTab}
                  setNavOpen={setNavOpen}
                  handleLogout={handleLogout}
                />
              </Hidden>
              <Hidden mdUp={true}>
                <NavBar
                  currNavTab={navTab}
                  drawerVariant="temporary"
                  open={navIsOpen}
                  setNavTab={setNavTab}
                  setNavOpen={setNavOpen}
                  handleLogout={handleLogout}
                />
              </Hidden>
            </>
          )}
          <Switch>
            {isLoggedIn && (
              <>
                {navPageRoutes}
                <Redirect exact from="/login" to="/inventory" />
              </>
            )}
            {!isFetching ? (
              <>
                <Route
                  path="/login"
                  render={() => <Login setLoggedIn={setLoggedIn} />}
                />
                <Route path="/signup" component={SignUp} />
                <Redirect to="/login" />
              </>
            ) : (
              <div
                style={{
                  minWidth: "100vw",
                  minHeight: "100vh",
                  display: "flex",
                  flexDirection: "column",
                  justifyContent: "center",
                  alignItems: "center",
                }}
              >
                <CircularProgress
                  variant="indeterminate"
                  size={theme.spacing(30)}
                  style={{
                    marginBottom: theme.spacing(10),
                    color: theme.palette.primary.light,
                  }}
                />
                <Typography variant="h4" color="textPrimary">
                  Loading
                </Typography>
              </div>
            )}
          </Switch>
        </ThemeProvider>
      </BrowserRouter>
    </div>
  );
}

export default App;
