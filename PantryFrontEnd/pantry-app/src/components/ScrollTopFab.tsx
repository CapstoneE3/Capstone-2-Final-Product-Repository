import {
  Box,
  Button,
  Fab,
  Slide,
  useScrollTrigger,
  useTheme,
  Zoom,
} from "@material-ui/core";
import { KeyboardArrowUp } from "@material-ui/icons";
import React from "react";
interface scrollTopProps {
  window?: () => Window;
  topRef: React.MutableRefObject<HTMLDivElement | null>;
}

const ScrollTopFab: React.FC<scrollTopProps> = ({ window, topRef }) => {
  //   const trigger = useScrollTrigger({
  //     target: window ? window() : undefined,
  //     threshold: 100,
  //     disableHysteresis: true,
  //   });
  const trigger = useScrollTrigger({ threshold: 99, disableHysteresis: true });
  const theme = useTheme();
  return (
    <Slide appear={false} in={trigger} direction="up" style={{ zIndex: 20 }}>
      <Box display="flex" justifyContent="center" alignItems="center">
        {/* <Button
          variant="contained"
          color="primary"
          style={{ position: "fixed", bottom: theme.spacing(2) }}
        >
          Hello
        </Button> */}
        <Fab
          style={{ position: "fixed", bottom: theme.spacing(2), opacity: 0.7 }}
          color="primary"
          size="small"
          onClick={() => {
            if (topRef.current)
              topRef.current.scrollIntoView({
                behavior: "smooth",
                block: "start",
              });
            // topRef.current.scrollTo({ top: 0, behavior: "smooth" });
          }}
        >
          <KeyboardArrowUp
            style={{ color: theme.palette.background.default }}
          />
        </Fab>
      </Box>
    </Slide>
  );
};

export default ScrollTopFab;
