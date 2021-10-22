import { CircularProgress, Container } from "@material-ui/core";
import React from "react";
import { Recipe } from "./mockEntries";
import RecipeEntry from "./RecipeEntry";

interface RecipeTabProps {
  activeTab: number;
  index: number;
  propEntries: Recipe[];
  handleOpenEdit: (recipe: Recipe, i: number) => void;
  handleRemove: (recipe: Recipe) => void;
  handleAdd: () => void;
}

const getKey = (name: string) => {
  var key = 0;
  for (let i = 0; i < name.length; i++) {
    key += name.charCodeAt(i);
  }

  return key;
};

const RecipeTab: React.FC<RecipeTabProps> = ({
  activeTab,
  index,
  propEntries,
  handleOpenEdit,
  handleRemove,
  handleAdd,
}) => {
  return (
    <>
      {activeTab === index && (
        <Container
          style={{
            maxWidth: "none",
            textAlign: "center",
            overflow: "hidden",
            marginBottom: "2rem",
          }}
        >
          {propEntries.length > 0 ? (
            propEntries.map((r, i) => {
              return (
                <RecipeEntry
                  key={getKey(r.name)}
                  i={i}
                  recipe={r}
                  handleOpenEdit={handleOpenEdit}
                  handleRemove={handleRemove}
                  handleAdd={handleAdd}
                ></RecipeEntry>
              );
            })
          ) : (
            <CircularProgress color="primary" size={100} />
          )}
        </Container>
      )}
    </>
  );
};

export default RecipeTab;
