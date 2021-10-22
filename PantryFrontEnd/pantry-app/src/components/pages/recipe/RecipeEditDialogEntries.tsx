import {
  ButtonGroup,
  ListItem,
  ListItemText,
  makeStyles,
  TextField,
  Theme,
} from "@material-ui/core";
import { Block, Edit } from "@material-ui/icons";
import { createStyles, useTheme } from "@material-ui/styles";
import React from "react";
import { StyledActionButton } from "./RecipeEntry";

interface EntryProps {
  i: number;
  ig: string;
  handleEdited: (i: number, name: string) => void;
  handleRemove: (i: number) => void;
}

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    editButton: {
      backgroundColor: theme.palette.secondary.main,
      "&:hover": {
        backgroundColor: theme.palette.secondary.light,
      },
    },

    removeButton: {
      backgroundColor: theme.palette.error.main,
      "&:hover": {
        backgroundColor: theme.palette.error.light,
      },
    },

    textFieldRoot: {
      [theme.breakpoints.down("xs")]: {
        // width: theme.spacing(15),
        width: "80%",
      },
    },
  })
);

const RecipeEditDialogEntries: React.FC<EntryProps> = ({
  i,
  ig,
  handleEdited,
  handleRemove,
}) => {
  const theme = useTheme();
  const classes = useStyles(theme);

  const [editable, setEditable] = React.useState(false);

  const [value, setValue] = React.useState(ig);

  const handleNameChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    setValue(e.target.value);
  };

  const handleNameEdited = (
    e: React.FocusEvent<HTMLTextAreaElement | HTMLInputElement>
  ) => {
    if (value) {
      setEditable(false);
      handleEdited(i, value);
    }
  };

  const handleSelfDestruct = (
    e: React.MouseEvent<HTMLButtonElement, MouseEvent>
  ) => {
    handleRemove(i);
  };

  return (
    <ListItem disableGutters>
      <ListItemText
        primary={
          <TextField
            // inputProps={{ className: classes.textFieldRoot }}
            required
            InputProps={{ className: classes.textFieldRoot }}
            disabled={!editable}
            value={value}
            maxRows={1}
            onChange={handleNameChange}
            onBlur={handleNameEdited}
            error={!value}
            helperText={!value ? "Field cannot be left empty" : ""}
          />
          //   <FormControl required disabled error variant="outlined">
          //     <Input
          //       // InputProps={{ className: classes.textFieldRoot }}
          //       className={classes.textFieldRoot}
          //       disabled={!editable}
          //       value={value}
          //       maxRows={1}
          //       onChange={handleNameChange}
          //       onBlur={handleNameEdited}
          //       error={!value}
          //       required
          //       // helperText={!value ? "Field cannot be left empty" : ""}
          //     />
          //   </FormControl>
        }
      />
      <ButtonGroup
        variant="contained"
        fullWidth
        size="large"
        style={{ width: "40%" }}
      >
        <StyledActionButton
          classes={{ root: classes.editButton }}
          onClick={() => setEditable(true)}
        >
          <Edit />
        </StyledActionButton>
        <StyledActionButton
          classes={{ root: classes.removeButton }}
          onClick={handleSelfDestruct}
        >
          <Block />
        </StyledActionButton>
      </ButtonGroup>
    </ListItem>
  );
};

export default RecipeEditDialogEntries;
