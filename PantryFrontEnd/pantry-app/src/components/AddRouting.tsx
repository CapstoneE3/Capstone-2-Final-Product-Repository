import React from "react";
import { RouteComponentProps, StaticContext } from "react-router";
import { pageToIndex } from "./routingTable";

interface HigherOrderProps {
  routeprops: RouteComponentProps<{}, StaticContext, unknown>;
  setNavTab: (page: number) => void;
  setNavOpen?: () => void;
  otherProps?: any[];
}

const AddRouting: (component: React.FC<any>) => React.FC<HigherOrderProps> = (
  component
) => {
  const Component = component;

  const HigherOrder: React.FC<HigherOrderProps> = ({
    routeprops,
    setNavTab,
    setNavOpen,
    otherProps,
  }) => {
    React.useEffect(() => {
      console.log("Rerendered higher order");
      const {
        match: { url: page },
      } = routeprops;

      setNavTab(pageToIndex(page));
    }, [routeprops, setNavTab]);

    return <Component setNavOpen={setNavOpen} {...otherProps}></Component>;
  };

  return HigherOrder;
};

export default AddRouting;
