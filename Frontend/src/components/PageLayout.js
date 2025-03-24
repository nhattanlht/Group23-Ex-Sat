import Sidebar from "./Sidebar";

const PageLayout = ({ children, title = "Dashboard" }) => {
  return (
    <div className="flex min-h-screen w-screen">
      <Sidebar />
      <div className="flex-1 ml-0 transition-all duration-300">
        <div className="p-6">
          <h1 className="text-2xl font-bold">{title}</h1>
          <div className="mt-6">
            {children}
          </div>
        </div>
      </div>
    </div>
  );
};

export default PageLayout;